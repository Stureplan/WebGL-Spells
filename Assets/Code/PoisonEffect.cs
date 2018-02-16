using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public GameObject draugr;
    public GameObject skeleton;
    public GameObject boneHierarchy;
    private Material material;

    float dissolve_timer = 0.5f;
    float timescale = 5.0f;
    public void StartDissolve()
    {
        material = draugr.GetComponent<SkinnedMeshRenderer>().material;
        StartCoroutine(Dissolve(dissolve_timer));

        GetComponentInChildren<ParticleSystem>().Play();
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

        material.SetFloat("_Dissolve", 1.2f);


        UnparentSkeleton();
        GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);
    }

    private void UnparentSkeleton()
    {
        Destroy(GetComponent<Animation>());
        Transform[] bones = boneHierarchy.GetComponentsInChildren<Transform>();
        for (int i = 0; i < bones.Length; i++)
        {
            //bones[i].SetParent(Spell.CleanupTransform());
            bones[i].gameObject.AddComponent<BoxCollider>().size = new Vector3(10, 2, 2);
            Rigidbody r = bones[i].gameObject.AddComponent<Rigidbody>();
            r.mass = Random.Range(0.5f, 1.5f);
            r.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.0f, 1.0f), Random.Range(0.5f, 1.5f)) * 7, ForceMode.Impulse);

        }


    }
}
