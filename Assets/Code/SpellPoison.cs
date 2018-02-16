using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPoison : Spell
{
    public float speed = 5.0f;

    public GameObject poisonImpact;
    public GameObject smokeImpact;

    public GameObject[] children;

    private Rigidbody rb;
    private Vector3 direction = Vector3.zero;

    public override void SetTarget(Vector3 pos)
    {
        target = new Vector3(pos.x, 2, pos.z);
        direction = (target - transform.position).normalized;

        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;

    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Draugr")
        {
            other.GetComponent<Animation>().Play("Death 1");
            other.GetComponent<PoisonEffect>().StartDissolve();
            GameObject go1 = Instantiate(poisonImpact, transform.position + transform.forward, transform.rotation);
            GameObject go2 = Instantiate(smokeImpact,  transform.position + transform.forward, transform.rotation);

            Destroy(go1, 4.0f);
            Destroy(go2, 4.0f);

            for (int i = 0; i < children.Length; i++)
            {
                UnparentAndDestroy(children[i]);
            }

            Destroy(gameObject);
        }
    }

    private void UnparentAndDestroy(GameObject go)
    {
        go.transform.SetParent(Spell.CleanupTransform());

        ParticleSystem.MainModule module = go.GetComponent<ParticleSystem>().main;
        module.loop = false;

        Destroy(go, 3.0f);
    }
}
