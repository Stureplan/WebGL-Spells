using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Vector3 target;
    public virtual void SetTarget(Vector3 pos) { }
}
