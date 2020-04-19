using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : Singleton<FireController> {
    public LayerMask layerMask;
    public float fireForce;
    public bool burning;
    private void Start() {
        StartCoroutine(Fire());
    }
    private IEnumerator Fire() {
        while (true) {
            float force = 0;
            Item tmp;
            foreach (Collider collisions in Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, layerMask)) {
                tmp = collisions.GetComponent<Item>();
                if (tmp.burning) {
                    force += tmp.force;
                }
            }
            fireForce = force;
            if (fireForce <= 0) {
                burning = false;
            } else {
                burning = true;
            }

            yield return new WaitForSeconds(.2f);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        //Draw the next cross position
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        
    }
}
