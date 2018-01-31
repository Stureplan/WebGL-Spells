using System.Collections;
using UnityEngine;

public class SpellFrostshard : Spell
{
    private Rigidbody rb;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;

    public GameObject[] stayingChildren;
    public GameObject frozenSphere;

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
        if (other.GetComponentInChildren<FrostshardEffect>() == null)
        {
            Animation anim = other.GetComponent<Animation>();
            anim.Stop();
            GameObject go = Instantiate(frozenSphere, other.transform.position, other.transform.rotation);
            go.transform.localScale = other.transform.localScale * 0.65f;
            go.transform.SetParent(other.transform);
            go.GetComponent<FrostshardEffect>().desiredScale = other.transform.localScale;
        }





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


        StartCoroutine(FadeShard(5.0f));

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

    }

    private IEnumerator FadeShard(float timer)
    {
        float t = 0.0f;

        Vector3 originalPos = transform.position;
        Vector3 lerpedPos = transform.position += (transform.forward * 0.5f);
        Vector3 originalScl = transform.localScale;

        while (t < timer)
        {
            transform.localScale = Vector3.Lerp(originalScl, Vector3.zero, t / timer);
            transform.position = Vector3.Lerp(originalPos, lerpedPos, t / timer);

            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
