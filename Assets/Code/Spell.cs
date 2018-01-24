using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float timeout = 5.0f;
    public Vector3 target;
    public virtual void SetTarget(Vector3 pos) { }
    private void Start()
    {
        Destroy(gameObject, timeout);
    }


    private static GameObject cleanupObject;
    public static Transform CleanupTransform()
    {
        if (cleanupObject == null)
        {
            cleanupObject = GameObject.FindGameObjectWithTag("Cleanup");
        }

        return cleanupObject.transform;
    }
}
