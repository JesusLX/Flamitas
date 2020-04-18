using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public Action OnMouseUpListener;


    public void OnMouseUp() {
        if (OnMouseUpListener != null) {
            OnMouseUpListener();
        }
    }
}
