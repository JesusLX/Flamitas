using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : Singleton<SpawnerController>
{
    public List<GameObject> poolItems;

    [Header("Items Prefabs")]
    public List<GameObject> ItemsPrefabs;


    private void Awake() {
        if(poolItems == null) {
            poolItems = new List<GameObject>();
        }
        if(poolItems.Count == 0) {
            poolItems.AddRange(ItemsPrefabs);

            poolItems.ForEach(o => o.SetActive(false));
        }
    }

    public void SpawnItem(int itemId) {
        GameObject item = poolItems.Find(o => !o.activeInHierarchy && o.GetComponent<Item>().ID == itemId);
        if(item == null) {
            GameObject prefab = ItemsPrefabs.Find(o => o.GetComponent<Item>().ID == itemId);
            item = Instantiate(prefab);
            poolItems.Add(item);
        }
        item.transform.position = new Vector3(item.transform.position.x, 3, item.transform.position.z);
        item.SetActive(true);
        item.GetComponent<Item>().canMove = true;
        item.GetComponent<Item>().dragging = true;
    }
}
