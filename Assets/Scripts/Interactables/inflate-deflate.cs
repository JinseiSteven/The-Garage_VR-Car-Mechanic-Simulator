using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public GameObject band;
    public Vector3 scaleChange;

    void Awake()
    {
        band.transform.localScale += scaleChange;

    }

    void Update()
    {

        // Move upwards when the sphere hits the floor or downwards
        // when the sphere scale extends 1.0f.

    }
}