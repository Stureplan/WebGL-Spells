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
    public ParticleSystem childFlamesSparks;
    public TrailRenderer childTrailRenderer;
    public Material burningMaterial;
    public GameObject sparksPrefab;
    public GameObject flamesStayingPrefab;

    public override void SetTarget(Vector3 pos)
    {
        target = new Vector3(pos.x, 1, pos.z);
        direction = (target - transform.position).normalized;

        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }

    private Color RandomColor()
    {
        float red= Random.Range(0.5f, 1.0f);
        float green = Random.Range(0.0f, 0.75f);
        float blue = Random.Range(0.0f, 0.25f);

        Color c = new Color(red, green, blue);
        return c;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void Explosion()
    {
        Collider[] hits;

        int layerMask = ~(1 << LayerMask.NameToLayer("Spell"));
        hits = Physics.OverlapSphere(transform.position, 4, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            Rigidbody temp = hits[i].GetComponent<Rigidbody>();
            if (temp != null)
            {
                temp.AddForce((Vector3.up * 15)+((hits[i].transform.position - transform.position).normalized * explosionForce), ForceMode.Impulse);
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

        FadeShaderProperty[] fades = childTrailRenderer.GetComponents<FadeShaderProperty>();
        for (int i = 0; i < fades.Length; i++)
        {
            fades[i].Fade();
        }

        childFlamesSparks.Stop();

        childFlamesLocal.transform.SetParent(Spell.CleanupTransform());
        childFlamesGlobal.transform.SetParent(Spell.CleanupTransform());
        childFlamesSparks.transform.SetParent(Spell.CleanupTransform());
        childTrailRenderer.transform.SetParent(Spell.CleanupTransform());

        Destroy(childFlamesLocal.gameObject, 3);
        Destroy(childFlamesGlobal.gameObject, 3);
        Destroy(childFlamesSparks.gameObject, 3);
        Destroy(childTrailRenderer.gameObject, 3);

        other.gameObject.AddComponent<FireballEffect>().material = burningMaterial;

        GameObject go = Instantiate(sparksPrefab, transform.localPosition, Quaternion.identity);
        Destroy(go, 3);

        GameObject go1 = Instantiate(flamesStayingPrefab, transform.localPosition, Quaternion.identity);
        Destroy(go1, 3);

        Destroy(gameObject);
    }
}
