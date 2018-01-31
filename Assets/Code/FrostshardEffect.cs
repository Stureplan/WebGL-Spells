using System.Collections;
using UnityEngine;

public class FrostshardEffect : MonoBehaviour 
{
    public Vector3 desiredScale = Vector3.zero;

    private void Start()
    {
        Destroy(gameObject, 5.0f);
        StartCoroutine(ScaleEffect(0.1f));
    }

    private void OnDestroy()
    {
        transform.parent.GetComponent<Animation>().Play();
        SpellFrostshard[] fx = transform.parent.GetComponentsInChildren<SpellFrostshard>();
        for (int i = 0; i < fx.Length; i++)
        {
            if (fx[i] != this)
            {
                Destroy(fx[i].gameObject);
            }
        }
    }


    private IEnumerator ScaleEffect(float timer)
    {
        float t = 0.0f;

        Vector3 originalScl = transform.localScale;

        while (t < timer)
        {
            transform.localScale = Vector3.Lerp(originalScl, desiredScale, t / timer);

            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = desiredScale;
    }
}
