using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFrostshard : Spell
{
    private Rigidbody rb;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;

    public GameObject[] stayingChildren;

    public override void SetTarget(Vector3 pos)
    {
        target = new Vector3(pos.x, 1, pos.z);
        direction = (target - transform.position).normalized;

        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }
    
    private void FixedUpdate()
    {
        if (rb != null)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Animation anim = other.GetComponent<Animation>();

        anim.Stop();


        for (int i = 0; i < stayingChildren.Length; i++)
        {
            stayingChildren[i].transform.SetParent(Spell.CleanupTransform());

            ParticleSystem ps = stayingChildren[i].GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ParticleSystem.MainModule module = ps.main;
                module.loop = false;
                ps.Stop();
                Destroy(stayingChildren[i], 3.0f);

            }

            TrailRenderer tr = stayingChildren[i].GetComponent<TrailRenderer>();
            if (tr != null)
            {
                StartCoroutine(FadeTrail(0.5f, stayingChildren[i], tr));
            }

        }

        transform.SetParent(other.transform);
        Destroy(GetComponent<BoxCollider>());
        Destroy(rb);
    }

    private IEnumerator FadeTrail(float timer, GameObject go, TrailRenderer tr)
    {
        float t = 0.0f;
        while (t < timer)
        {
            Color ec = tr.endColor;
            float sw = tr.startWidth;


            tr.endColor = Color.Lerp(ec, Color.clear, t / timer);
            tr.startWidth = Mathf.Lerp(sw, 0.0f, t / timer);

            t += Time.deltaTime;
            yield return null;
        }

        Destroy(go);
        Destroy(this);
    }
}
