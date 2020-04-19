using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : Singleton<ShopManager> {

    public TextMeshProUGUI coinText;

    private void Awake() {
        updateCoinsText();
    }

    public void updateCoinsText() {
        coinText.text = GameManager.Instance.coins.ToString();
    }

    public void buyItem(int itemID) {
        // Comprobamos que nuestro dinero actual es igual o superior al precio del item a comprar
        if (GameManager.Instance.coins >= SpawnerController.Instance.ItemsPrefabs.Find(o => o.GetComponent<Item>().ID == itemID).GetComponent<Item>().Price) {

            if (!GameManager.Instance.inventoryItems.ContainsKey(itemID)) {
                GameManager.Instance.inventoryItems.Add(itemID, 0);
            }

            if (GameManager.Instance.inventoryItems[itemID] < 0) {
                GameManager.Instance.inventoryItems[itemID] = 0;
            }

            GameManager.Instance.inventoryItems[itemID] += 1;

            UIManager.Instance.UpdateItem(itemID, GameManager.Instance.inventoryItems[itemID]);

            GameManager.Instance.updateCoins(-SpawnerController.Instance.ItemsPrefabs.Find(o => o.GetComponent<Item>().ID == itemID).GetComponent<Item>().Price);
        }

    }
}
