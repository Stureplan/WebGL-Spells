using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : MonoBehaviour
{
    /* ===== Spell List */
    public List<GameObject> spellPrefabs;
    public GameObject currentSpell;
    private int current = 0;

    /* ===== Casting    */
    public Transform castOrigin;
    public Camera castCamera;

    public void PreviousSpell()
    {
        current--;
        if (current < 0)
        {
            current = spellPrefabs.Count - 1;
        }

        currentSpell = spellPrefabs[current];
    }

    public void NextSpell()
    {
        current++;
        if (current >= spellPrefabs.Count)
        {
            current = 0;
        }

        currentSpell = spellPrefabs[current];
    }

    public void CastSpell()
    {
        Vector3 target = FindTarget();
        Quaternion rot = Quaternion.LookRotation((target-castOrigin.position).normalized, Vector3.up);
        GameObject spellObject = Instantiate(currentSpell, castOrigin.position, rot);

        Spell spell = spellObject.GetComponent<Spell>();
        spell.SetTarget(target);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastSpell();
        }
    }

    private Vector3 FindTarget()
    {
        RaycastHit hit;
        Ray ray;
        Vector3 point = Vector3.zero;
        
        ray = castCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            point = hit.point;
        }

        return point;
    }
}
