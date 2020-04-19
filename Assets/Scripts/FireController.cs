using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : Singleton<FireController> {
    public LayerMask layerMask;
    public float fireForce;
    public bool burning;
    public ParticleSystem parentPS;
    public List<ParticleSystem> childsPS;
    float maxRadius = .4f;
    float maxParticlesEmision = 30;


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
                parentPS.Stop();
            } else {
                burning = true;
                if (!parentPS.isPlaying)
                    parentPS.Play();
                foreach (ParticleSystem ps in childsPS) {
                    var mEmision = ps.emission;
                    var mShape = ps.shape;
                    mEmision.rateOverTime = force;
                    mShape.radius = force * maxRadius / maxParticlesEmision;
                }
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
