using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : Singleton<SpawnerController> {
    public List<GameObject> poolItems;

    [Header("Items Prefabs")]
    public List<GameObject> ItemsPrefabs;


    private void Awake() {
        if (poolItems == null) {
            poolItems = new List<GameObject>();
        }
        if (poolItems.Count == 0) {
            foreach (GameObject item in ItemsPrefabs) {
                poolItems.Add(Instantiate(item));
            }

            poolItems.ForEach(o => o.SetActive(false));
        }
    }

    public GameObject SpawnItem(int itemId, bool centre = false) {
        GameObject item = poolItems.Find(o => !o.activeInHierarchy && o.GetComponent<Item>().ID == itemId);
        if (item == null) {
            GameObject prefab = ItemsPrefabs.Find(o => o.GetComponent<Item>().ID == itemId);
            item = Instantiate(prefab);
            poolItems.Add(item);
        }
        //item.transform.position = new Vector3(item.transform.position.x, 3, item.transform.position.z);
        if (centre) {
            item.transform.position = transform.position;
            item.GetComponent<Item>().dragging = false;
        } else {
            item.GetComponent<Item>().dragging = true;
        }
        item.SetActive(true);
        item.GetComponent<Item>().canMove = true;
        GameManager.Instance.OnItemSpawned(itemId);
        return item;
    }
    public void MovePickedItemToSpawn() {
        GameObject item = poolItems.Find(o => o.GetComponent<Item>().dragging == true);
        if (item) {
            item.transform.position = this.gameObject.transform.position;
            item.GetComponent<Item>().dragging = false;
        }
    }
    public int DisablePickedItems(int id) {
        int count = poolItems.FindAll(o => (o.GetComponent<Item>().dragging == true && o.GetComponent<Item>().ID == id)).Count;
        poolItems.FindAll(o => (o.GetComponent<Item>().dragging == true && o.activeInHierarchy == true)).ForEach(o => o.SetActive(false));
        return count;
    }
}
