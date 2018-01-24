using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public GameObject draugr;
    public GameObject skeleton;
    public GameObject boneHierarchy;
    private Material material;

    float dissolve_timer = 1.1f;
    float timescale = 1.0f;
    public void StartDissolve()
    {
        material = draugr.GetComponent<SkinnedMeshRenderer>().material;
        StartCoroutine(Dissolve(dissolve_timer));
    }

    private IEnumerator Dissolve(float timer)
    {
        float t = 0.0f;
        while (t < timer)
        {
            material.SetFloat("_Dissolve", t);
            t += Time.deltaTime * timescale;
            yield return null;
        }

        UnparentSkeleton();
    }

    private void UnparentSkeleton()
    {
        Destroy(GetComponent<Animator>());
        Transform[] bones = boneHierarchy.GetComponentsInChildren<Transform>();
        for (int i = 0; i < bones.Length; i++)
        {
            //bones[i].SetParent(Spell.CleanupTransform());
            bones[i].gameObject.AddComponent<BoxCollider>().size = new Vector3(10, 2, 2);
            bones[i].gameObject.AddComponent<Rigidbody>();

        }


    }
}
