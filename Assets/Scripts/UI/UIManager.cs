using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public void SpawnItem(int itemId) {
        SpawnerController.Instance.SpawnItem(itemId);
    }

    public void HandsUp() {
        GameManager.Instance.OnMouseUp();
    }
}
