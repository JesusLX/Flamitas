using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {
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
            foreach (Collider collisions in Physics.OverlapBox(transform.position, Vector3.one, transform.rotation, layerMask)) {
                tmp = collisions.GetComponent<Item>();
                if (tmp.burning) {
                    force += tmp.force;
                }
            }
            fireForce = force;
            if (fireForce <= 0) {
                burning = false;
            }

            yield return new WaitForSeconds(.2f);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        //Draw the next cross position
        Gizmos.DrawWireCube(transform.position,Vector3.one*4);
        
    }
}
