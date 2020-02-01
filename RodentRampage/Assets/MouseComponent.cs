using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseComponent : MonoBehaviour
{
    public float gnawSpeed = 0.5f;

    private PlayerInput input = null;

    private bool gnawing = false;
    private MachineComponent touchingMachine = null;

    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (touchingMachine && gnawing)
            touchingMachine.gnaw(gnawSpeed * Time.deltaTime);
    }

    void OnUse(InputValue value)
    {
        gnawing = value.Get<float>() > 0.5f;
    }

    void OnTriggerEnter(Collider other)
    {
        var machine = other.transform.parent.GetComponent<MachineComponent>();
        if (machine)
        {
            touchingMachine = machine;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var machine = other.transform.parent.GetComponent<MachineComponent>();
        if (machine && touchingMachine == machine)
        {
            touchingMachine = null;
        }
    }
}
