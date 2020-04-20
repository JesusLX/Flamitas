using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public float nextBurn = .5f;

    private void Awake() {
        collisioningItems = new List<Item>();
        GameManager.Instance.OnMouseUpListener += OnMouseUp;
        _rb = GetComponent<Rigidbody>();
        GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().material);
        canMove = true;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnMouseUpListener -= OnMouseUp;
        }
    }
    public void Init() {
        Debug.Log("Init");
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
        if (transform.position.y < 0) {
            transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        }
        if (canMove) {
            if (dragging) {
                Vector3 mouse;
#if UNITY_EDITOR
                mouse = Input.mousePosition;
#else
                mouse = /*Input.mousePosition;*/Input.GetTouch(0).position;
#endif
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, layerMask)) {

                    transform.position = new Vector3(hit.point.x, Mathf.Clamp(transform.position.y, 0.5f, 5), hit.point.z);
                }
                // if () {
                //float speed = 10F; transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * speed);

                //}
            }
        }
        if (burning) {
            if (GetComponent<Renderer>().material.GetFloat("Slider_Fill") != nextBurn) {
                //Debug.Log("De " + GetComponent<Renderer>().material.GetFloat("Slider_fll") + " a " + nextBurn + " -> " + Mathf.Lerp(GetComponent<Renderer>().material.GetFloat("Slider_fll"), nextBurn, Time.deltaTime));
                float next = Mathf.Lerp(GetComponent<Renderer>().material.GetFloat("Slider_Fill"), nextBurn, Time.deltaTime);
                Debug.Log(next);
                GetComponent<Renderer>().material.SetFloat("Slider_Fill", next);
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
        nextBurn = ((MaxDuration - curDuration) / MaxDuration) - .55f;
    }
    public void Hide() {
        burning = false;
        GetComponent<Renderer>().material.SetFloat("Slider_Fill", -.85f);

        GetComponent<Collider>().enabled = false;
        gameObject.SetActive(false);
    }
}
