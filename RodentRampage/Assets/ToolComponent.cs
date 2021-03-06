﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType { Hammer, Wrench, Screwdriver, Drill, Oilcan };

public class ToolComponent : MonoBehaviour
{
    public ToolType toolType;
    public MechanicComponent pickedUpBy;
    public float friction = 0.7f;

    void Start()
    {
    }

    void FixedUpdate()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity -= friction * rb.velocity * Time.fixedDeltaTime;
        rb.velocity += 9.81f * Time.fixedDeltaTime * Vector3.down;
    }

    void Update()
    {
    }

}
