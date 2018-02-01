using System.Collections;
using UnityEngine;

public class FrostshardEffect : MonoBehaviour
{
    public Vector3 desiredScale = Vector3.zero;

    private void Start()
    {
        Destroy(gameObject, 2.0f);
        StartCoroutine(ScaleEffect(desiredScale, 0.1f));
    }

    private void OnDestroy()
    {
        //transform.parent.GetComponent<Animation>().Play();
        transform.parent.GetComponent<Animation>()["BallBounce"].speed = 1.0f;

        SpellFrostshard[] fx = transform.parent.GetComponentsInChildren<SpellFrostshard>();
        for (int i = 0; i < fx.Length; i++)
        {
            if (fx[i] != this)
            {
                Destroy(fx[i].gameObject);
            }
        }
    }


    private IEnumerator ScaleEffect(Vector3 dScale, float timer)
    {
        float t = 0.0f;

        Vector3 originalScl = transform.localScale;

        while (t < timer)
        {
            transform.localScale = Vector3.Lerp(originalScl, dScale, t / timer);

            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = dScale;
    }
}
