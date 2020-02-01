using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolComponent : MonoBehaviour
{
    public enum ToolType { Hammer, Wrench, Screwdriver, Drill, Oilcan };
    public ToolType toolType;
    public MechanicComponent pickedUpBy;
    public float friction = 1.0f;

    void Start()
    {
    }

    void FixedUpdate()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity -= friction * rb.velocity * Time.fixedDeltaTime;
    }

    void Update()
    {
        if (pickedUpBy)
        {
            var pos = pickedUpBy.transform.localPosition;
            pos.y += 0.5f;
            transform.localPosition = pos;
        }
    }
}
