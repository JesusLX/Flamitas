using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemController : MonoBehaviour {
    public TextMeshProUGUI txtCount;
    public Image image;
    public int ID;
    EventTrigger eventTrigger;
    private bool lastWasClicked = false;
    private bool lastWasDown = false;
    Vector3 downPos;
    bool timming = false;
    float timer = 0;
    float time = 0.2f;
    private void Awake() {
        eventTrigger = GetComponent<EventTrigger>();
    }
    private void Update() {
        if (timming) {
            timer += Time.deltaTime;
        }
    }
    public void SetOnPointerAction() {
        eventTrigger.triggers.Clear();

        SetOnPointer(EventTriggerType.PointerClick, (e) => Click());
        SetOnPointer(EventTriggerType.PointerDown, (e) => Down());
        SetOnPointer(EventTriggerType.PointerUp, (e) => Up());
    }
    private void SetOnPointer(EventTriggerType type, UnityAction<BaseEventData> action) {
        var pointerEntry = new EventTrigger.Entry();
        pointerEntry.eventID = type;
        pointerEntry.callback.AddListener(action);
        eventTrigger.triggers.Add(pointerEntry);
    }
    private void Click() {
        if (lastWasDown) {
            lastWasDown = true;
        } else {
            Debug.Log("Clicked");
            UIManager.Instance.SpawnItem(ID, true);
            lastWasClicked = true;
        }
    }
    private void Down() {
        if (lastWasClicked) {
            lastWasClicked = false;
        } else {
            downPos = GetPointClicked();
            Debug.Log("Down");
            UIManager.Instance.SpawnItem(ID);
            lastWasDown = true;
        }
        timming = true;
        timer = 0;
    }
    private void Up() {
        Debug.Log(downPos + " : " + GetPointClicked() + " ---- " + Vector3.Distance(downPos, GetPointClicked()));
        if (Vector3.Distance(downPos, GetPointClicked()) < 1f && timer <= time) {
            SpawnerController.Instance.MovePickedItemToSpawn();
            UIManager.Instance.TryHide(ID);
        } else if (Vector3.Distance(downPos, GetPointClicked()) < 1f && timer >= time) {
            GameManager.Instance.RefundItem(ID);
        } else {
            UIManager.Instance.HandsUp();
            UIManager.Instance.TryHide(ID);
        }
        timer = 0;
        timming = false;
    }
    private Vector3 GetPointClicked() {
        Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1)) {
            return hit.point;
        }
        return Vector3.negativeInfinity;
    }
}
