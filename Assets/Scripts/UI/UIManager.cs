using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
    public GameObject InventoryLayout;

    [Header("Prefabs")]
    public GameObject itemsButPref;
    public void SpawnItem(int itemId,bool centre = false) {
        SpawnerController.Instance.SpawnItem(itemId,centre);
        if (centre) {
            Debug.Log("centrando");
        }
    }

    public void HandsUp() {
        GameManager.Instance.OnMouseUp();
    }

    internal void UpdateItems() {
        ClearInventory();
        foreach (int itemID in GameManager.Instance.inventoryItems.Keys) {
            UpdateItem(itemID, GameManager.Instance.inventoryItems[itemID]);
        }
    }

    public void UpdateItem(int itemID, int count) {
        Transform tItem = InventoryLayout.transform.Find(itemID.ToString());
        GameObject item;
        if(tItem == null) {
            item = Instantiate(itemsButPref, InventoryLayout.transform);
            UIItemController itemCont = item.GetComponent<UIItemController>();
            itemCont.image.sprite = SpawnerController.Instance.ItemsPrefabs.Find(o => o.GetComponent<Item>().ID == itemID).GetComponent<Item>().mainSprite;
            itemCont.ID = itemID;
            itemCont.SetOnPointerAction();
            item.name = itemID.ToString();
        } else {
            item = tItem.gameObject;
        }
        item.GetComponent<UIItemController>().txtCount.text = count.ToString();


        if (count == -1) {
            item.SetActive(false);
        }
    }
    public void HideItem(int itemID) {
        UpdateItem(itemID,-1);
    }
    public void TryHide(int itemID) {
        if (GameManager.Instance.inventoryItems[itemID] <= 0) {
            HideItem(itemID);
        }
    }

    public void ClearInventory() {
        for (int i = InventoryLayout.transform.childCount - 1; i >= 0; i--) {
            Destroy(InventoryLayout.transform.GetChild(i).gameObject);
        }
    }
}
