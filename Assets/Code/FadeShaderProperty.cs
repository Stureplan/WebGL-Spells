using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeShaderProperty : MonoBehaviour
{
    public Renderer rendererComponent;
    private Material material;
    public string property;
    public float startValue;
    public float endValue;
    public float power = 1;

	public void Fade()
    {
        material = rendererComponent.material;

        if (startValue < endValue)
        {
            StartCoroutine(FadeIn(property, startValue, endValue));
        }
        else
        {
            StartCoroutine(FadeOut(property, startValue, endValue));
        }
    }

    private IEnumerator FadeIn(string property, float start, float end)
    {
        material.SetFloat(property, start);

        float t = start;

        while (t < end)
        {
            t += (Time.deltaTime * power);
            material.SetFloat(property, t);
            yield return null;
        }

        material.SetFloat(property, end);
    }
    private IEnumerator FadeOut(string property, float start, float end)
    {
        material.SetFloat(property, start);

        float t = start;

        while (t > end)
        {
            t -= (Time.deltaTime*power);
            material.SetFloat(property, t);
            yield return null;
        }

        material.SetFloat(property, end);
    }
}