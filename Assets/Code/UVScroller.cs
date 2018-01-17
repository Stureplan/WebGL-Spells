using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour
{
    public Material mat;
    public Vector2 speed;

    private Vector2 offset;

    private void Start()
    {
        StartCoroutine(CycleUV());
    }

    private IEnumerator CycleUV()
    {
        float t = 0.0f;
        while (true)
        {
            t += Time.deltaTime;

            offset = new Vector2(t * speed.x, t * speed.y);

            mat.SetTextureOffset("_MainTex", offset);

            yield return null;
        }
    }
}
