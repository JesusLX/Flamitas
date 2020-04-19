using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager> {

    public TextMeshProUGUI coinText;
    public GameObject layoutShopPanel;
    public GameObject UIShopItemPrefab;

    private bool showingShop = false;

    private void Start() {
        // Supuestamente solo se llama una vez :)
        fillLayoutShopPanel();
    }
    private void Awake() {
        updateCoinsText();
    }

    public void updateCoinsText() {
        coinText.text = GameManager.Instance.coins.ToString();
    }

    public void buyItem(int itemID) {
        Debug.Log("La llamacion 1 /" + itemID);

        // Comprobamos que nuestro dinero actual es igual o superior al precio del item a comprar
        if (GameManager.Instance.coins >= SpawnerController.Instance.ItemsPrefabs.Find(o => o.GetComponent<Item>().ID == itemID).GetComponent<Item>().Price) {

            Debug.Log("La llamacion 2 /" + itemID);
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

    public void fillLayoutShopPanel() {
        clearShopList();

        foreach (GameObject item in SpawnerController.Instance.ItemsPrefabs) {
            Debug.Log("Me llamaron");
            Item tmp = item.GetComponent<Item>();

            GameObject prefab = Instantiate(UIShopItemPrefab, layoutShopPanel.transform);
            prefab.GetComponent<Image>().sprite = tmp.mainSprite;
            prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tmp.Price.ToString();

            prefab.GetComponent<Button>().onClick.RemoveAllListeners();
            int tmpID = tmp.ID;
            prefab.GetComponent<Button>().onClick.AddListener(() => buyItem(tmpID));
        }
    }

    private void clearShopList() {
        Debug.Log("me llamaron");
        for (int i = layoutShopPanel.transform.childCount - 1; i >= 0; i--) {
            Destroy(layoutShopPanel.transform.GetChild(i).gameObject);
        }
    }

    public void showShop() {
        showingShop = !showingShop;
        layoutShopPanel.transform.parent.gameObject.SetActive(showingShop);
    }
}
