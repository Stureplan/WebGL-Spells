using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 axis;
    public float angle;

    private void Update()
    {
        transform.Rotate(axis, angle);
    }
}
