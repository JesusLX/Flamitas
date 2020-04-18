using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int ID;
    public ItemType type;
    public float MaxDuration;
    public float duration;
    public float resistence;
    public float force;
    public bool canMove;
    public bool dragging;
    public bool burning;
    private Rigidbody _rb;
    public LayerMask layerMask;
    public Sprite mainSprite;
    public List<Item> collisioningItems; 
    private void Awake() {
        collisioningItems = new List<Item>();
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
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, layerMask)) {
            Debug.Log("Levanto en "+ hit.transform.tag);
            
        }
    }

    private void Update() {
        if (canMove) {
            if (dragging) {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, layerMask)) {
                    Debug.Log(hit.collider.gameObject.tag);
                    transform.position = new Vector3(hit.point.x, Mathf.Clamp(transform.position.y, 0.5f, 5), hit.point.z);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        
    }
    private void OnCollisionExit(Collision collision) {
        
    }

}
