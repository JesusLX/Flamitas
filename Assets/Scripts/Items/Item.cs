using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int ID;
    public float duration;
    public float resistence;
    public float force;
    public bool canMove;
    public bool dragging;
    public bool burning;
    private Rigidbody _rb;
    public LayerMask layerMask;
    private void Awake() {
        _rb = GetComponent<Rigidbody>();

        canMove = true;
    }
    private void OnMouseDown() {
        dragging = true;
    }
    private void OnMouseUp() {
        dragging = false;
    }

    private void Update() {
        if (canMove) {
            if (dragging) {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity,layerMask)) {
                    transform.position = new Vector3(hit.point.x,transform.position.y,hit.point.z);
                }
            }
        }
    }

}
