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

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchingMachine && gnawing)
            touchingMachine.brokenness += gnawSpeed * Time.deltaTime;
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
        if (touchingMachine == other)
            touchingMachine = null;
    }
}
