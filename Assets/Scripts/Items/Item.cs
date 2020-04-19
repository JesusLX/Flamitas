using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int ID;
    public ItemType type;
    public float MaxDuration;
    public float curDuration;
    public float resistence;
    public float force;
    public int Price;
    public bool canMove;
    public bool dragging;
    public bool burning;
    private Rigidbody _rb;
    public LayerMask layerMask;
    public Sprite mainSprite;
    public List<Item> collisioningItems;
    FireController fireZone;

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
    public void Init() {
        curDuration = MaxDuration;
        if (type == ItemType.Lighter) {
            burning = true;
        } 
        canMove = true;
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
            Debug.Log("Levanto en " + hit.transform.tag);

        }
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

    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.gameObject.layer == this.gameObject.layer) {
    //        collisioningItems.Add(collision.gameObject.GetComponent<Item>());
    //            StopCoroutine(Burn());
    //            StartCoroutine(Burn());
    //    }
    //}
    //private void OnCollisionExit(Collision collision) {
    //    if (collision.gameObject.layer == this.gameObject.layer) {
    //        collisioningItems.Remove(collision.gameObject.GetComponent<Item>());

    //    }
    //}

    private void OnTriggerEnter(Collider collider) {
        if (collider.TryGetComponent<FireController>(out fireZone)) {
            StopCoroutine(Burn());
            StartCoroutine(Burn());
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<FireController>(out fireZone)) {
            StopCoroutine(Burn());
        }
    }
    IEnumerator Burn() {
        while (true) {
            if (!burning) {
                float currentPower = 0;
                //collisioningItems.FindAll(o => o.burning == true).ForEach(o => currentPower += o.force);
                Debug.Log("Current damage " + currentPower);
                if (FireController.Instance.fireForce >= resistence) {
                    burning = true;
                }
            }
            if (burning) {
                addDuraton(-(.1f));
            }

            if (curDuration <= 0) {
                if (type == ItemType.Consumable || FireController.Instance.fireForce > resistence)
                    Hide();
            }
            yield return new WaitForSeconds(.2f);
        }
    }

    public void addDuraton(float dur) {
        curDuration += dur;
    }
    public void Hide() {
        GetComponent<Collider>().enabled = false;
        gameObject.SetActive(false);
    }

}
