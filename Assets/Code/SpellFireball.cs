using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFireball : Spell
{
    private Vector3 direction = Vector3.zero;
    private float speed = 10.0f;

    public override void SetTarget(Vector3 pos)
    {
        target = pos;
        direction = (target - transform.position).normalized;
    }


    private void FixedUpdate()
    {
        transform.position += (direction * speed * Time.deltaTime);
    }
}
