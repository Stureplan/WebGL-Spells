using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFireball : Spell
{
    private Rigidbody rb;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;
    public float explosionForce = 10.0f;

    public ParticleSystem childFlamesLocal;
    public ParticleSystem childFlamesGlobal;
    public TrailRenderer childTrailRenderer;

    public override void SetTarget(Vector3 pos)
    {
        target = new Vector3(pos.x, 1, pos.z);
        direction = (target - transform.position).normalized;

        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void Explosion()
    {
        Collider[] hits;

        hits = Physics.OverlapSphere(transform.position, 4);
        for (int i = 0; i < hits.Length; i++)
        {
            Rigidbody temp = hits[i].GetComponent<Rigidbody>();
            if (temp != null)
            {
                temp.AddForce((hits[i].transform.position - transform.position).normalized * explosionForce, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Explosion();


        ParticleSystem.MainModule module_local = childFlamesLocal.main;
        module_local.loop = false;
        module_local.startSize = 2.5f;
        module_local.startLifetime = 1;

        ParticleSystem.MainModule module_global = childFlamesGlobal.main;
        module_global.loop = false;

        childFlamesLocal.transform.SetParent(null);
        childFlamesGlobal.transform.SetParent(null);
        childTrailRenderer.transform.SetParent(null);

        Destroy(childFlamesLocal.gameObject, 1.5f);
        Destroy(childFlamesGlobal.gameObject, 1.5f);
        Destroy(childTrailRenderer.gameObject, 1.5f);

        Destroy(gameObject);
    }
}
