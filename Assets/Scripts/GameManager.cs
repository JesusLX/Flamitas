using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public Action OnMouseUpListener;
    public Dictionary<int /*Item ID*/, int/*Count*/> inventoryItems;
    public int coins;
    public float secondsTimer = 0;
    float secondsTime = 1;

    private void Awake() {
        inventoryItems = new Dictionary<int, int>();
        //Adding initial items
        inventoryItems.Add(0, 3);//Adding little sticks
        inventoryItems.Add(1, 1);//Adding wood log
        inventoryItems.Add(2, 1);//Adding lighter
        UIManager.Instance.UpdateItems();
    }
    private void Update() {
        if (FireController.Instance.burning && FireController.Instance.fireForce > 0) {
            if (secondsTimer <= secondsTime) {
                secondsTimer += Time.deltaTime;
            } else {
                updateCoins(1);
                secondsTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (ShopManager.Instance.showingShop) {
                ShopManager.Instance.showShop();
            } else {
                Application.Quit();
            }
        }
    }
    public void OnMouseUp() {
        if (OnMouseUpListener != null) {
            OnMouseUpListener();
        }
    }

    public void OnItemSpawned(int itemId) {
        inventoryItems[itemId]--;
        //if (inventoryItems[itemId] < 0) {
        //    inventoryItems[itemId] = 0;
        //}
        UIManager.Instance.UpdateItem(itemId, inventoryItems[itemId]);
    }

    public void RefundItem(int itemId) {
        inventoryItems[itemId] += SpawnerController.Instance.DisablePickedItems(itemId);
        UIManager.Instance.UpdateItem(itemId, inventoryItems[itemId]);
    }

    public void updateCoins(int newCoins) {
        this.coins += newCoins;
        ShopManager.Instance.updateCoinsText();
    }
}
