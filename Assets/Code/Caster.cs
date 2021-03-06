﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Caster : MonoBehaviour
{
    /* ===== Spell List */
    public List<GameObject> spellPrefabs;
    public List<GameObject> targetPrefabs;
    public GameObject currentSpell;
    public GameObject currentTarget;
    public GameObject cleanupObject;
    private int current = 0;

    /* ===== Casting    */
    public Transform castOrigin;
    public Camera castCamera;
    public ParticleSystem spellFlame;
    private Material spellFlameMaterial;

    private bool playing = true;
    private Color currentFlameColor;
    private float colorTimer = 0.5f;
    private Coroutine coroutine;

    private void Start()
    {
        spellFlameMaterial = spellFlame.GetComponent<Renderer>().material;
    }

    public void PreviousSpell()
    {
        current--;
        if (current < 0)
        {
            current = spellPrefabs.Count - 1;
        }

        currentSpell = spellPrefabs[current];
        if (currentTarget != null)
        {
            Destroy(currentTarget);
            foreach (Transform t in cleanupObject.transform)
            {
                Destroy(t.gameObject);
            }
        }
        currentTarget = Instantiate(targetPrefabs[current]);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ChangeFlameColor(colorTimer, currentSpell.GetComponent<Spell>().spellColor));
    }

    public void NextSpell()
    {
        current++;
        if (current >= spellPrefabs.Count)
        {
            current = 0;
        }

        currentSpell = spellPrefabs[current];
        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }
        currentTarget = Instantiate(targetPrefabs[current]);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ChangeFlameColor(colorTimer, currentSpell.GetComponent<Spell>().spellColor));
    }

    private IEnumerator ChangeFlameColor(float timer, Color target)
    {
        float t = 0.0f;

        while (t < timer)
        {
            t += Time.deltaTime;
            spellFlameMaterial.color = Color.Lerp(currentFlameColor, target, t / timer);
            yield return null;
        }

        currentFlameColor = spellFlameMaterial.color;
    }

    public void CastSpell()
    {
        Vector3 target = FindTarget();
        Quaternion rot = Quaternion.LookRotation((target-castOrigin.position).normalized, Vector3.up);
        GameObject spellObject = Instantiate(currentSpell, castOrigin.position, rot);
        spellObject.transform.SetParent(Spell.CleanupTransform());

        Spell spell = spellObject.GetComponent<Spell>();
        spell.SetTarget(target);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // No crappy UI hits, ignore them.
                return;
            }


            CastSpell();
        }

        if (Input.GetMouseButtonDown(1))
        {
            playing = !playing;
            if (playing == false)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
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
