﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEffect : MonoBehaviour
{
    public Material material;
    public Vector2 uv_speed = new Vector2(-1, -2);
    private Vector2 uv_offset;
    MeshRenderer mr;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        Material[] materials = mr.materials;
        Material[] newMaterials = new Material[materials.Length+1];

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] != null)
            {
                newMaterials[i] = materials[i];
            }
        }

        newMaterials[newMaterials.Length-1] = material;
        mr.materials = newMaterials;
        material = mr.materials[mr.materials.Length - 1];
        material.SetFloat("_Multiplier", 1.5f);

        StartCoroutine(Timer(material, 1.5f));
        StartCoroutine(Scroller(material));
    }

    private IEnumerator Timer(Material mat, float timer)
    {
        float t = timer;
        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            mat.SetFloat("_Multiplier", t);

            yield return null;
        }

        Material[] oldMaterials = new Material[mr.materials.Length-1];
        for (int i = 0; i < oldMaterials.Length; i++)
        {
            oldMaterials[i] = mr.materials[i];
        }

        mr.materials = oldMaterials;

        Destroy(this);
    }

    private IEnumerator Scroller(Material mat)
    {
        float t = 0.0f;
        while(true)
        {
            t += Time.deltaTime;

            uv_offset = new Vector2(t * uv_speed.x, t * uv_speed.y);
            mat.SetTextureOffset("_MainTex", uv_offset);

            yield return null;
        }
    }
}