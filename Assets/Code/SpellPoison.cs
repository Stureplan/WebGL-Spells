using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPoison : Spell
{
    public float speed = 5.0f;

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
            other.GetComponent<Animator>().SetTrigger("Death");
            other.GetComponent<PoisonEffect>().StartDissolve();
        }
    }
}
