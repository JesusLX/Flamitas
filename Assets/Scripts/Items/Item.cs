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
        GameManager.Instance.OnMouseUpListener += OnMouseUp;
        _rb = GetComponent<Rigidbody>();

        canMove = true;
    }
    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnMouseUpListener -= OnMouseUp;
        }
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
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, layerMask)) {
                    transform.position = new Vector3(hit.point.x, Mathf.Clamp(transform.position.y, 0.5f, 5), hit.point.z);
                }
            }
        }
    }

}
