using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechanicComponent : MonoBehaviour
{
    public float throwYDir = 0.2f;
    public float throwVelocity = 10.0f;
    public float throwOffsetLen = 2.0f;
    public float repairSpeed = 0.7f;

    public ToolComponent pickedUpTool;

    private PlayerInput input;
    private ToolComponent touchingTool;
    private MachineComponent touchingMachine;
    private bool repairing;

    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void OnThrow(InputValue inputValue)
    {
        if (!pickedUpTool && touchingTool && !touchingTool.pickedUpBy)
        {
            pickedUpTool = touchingTool;
            touchingTool.pickedUpBy = this;

            var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        else if (pickedUpTool)
        {
            var moveDir = GetComponent<PlayerMovement>().getMoveDir();
            if (moveDir.magnitude < 1e-5)
                moveDir = transform.rotation * Vector3.forward;
            var throwDirection = new Vector3(moveDir.x, throwYDir, moveDir.z);

            var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
            rb.velocity = throwVelocity * throwDirection;
            rb.position += throwOffsetLen * new Vector3(moveDir.x, 0.0f, moveDir.y);

            rb.isKinematic = false;
            rb.detectCollisions = true;

            pickedUpTool.pickedUpBy = null;
            pickedUpTool = null;
        }

    }

    void OnUse(InputValue value)
    {
        repairing = value.Get<float>() > 0.5f;
    }

    void OnTriggerEnter(Collider other)
    {
        var tool = other.GetComponent<ToolComponent>();
        if (tool)
        {
            touchingTool = tool;
        }

        var machine = other.transform.parent.GetComponent<MachineComponent>();
        if (machine)
        {
            touchingMachine = machine;
        }
    }

    void onTriggerExit(Collider other)
    {
        var tool = other.GetComponent<ToolComponent>();
        if (tool && touchingTool == tool)
        {
            touchingTool = null;
        }

        var machine = other.transform.parent.GetComponent<MachineComponent>();
        if (machine && touchingMachine == machine)
        {
            touchingMachine = null;
        }
    }

    void Update()
    {
        if (touchingMachine && repairing)
        {
            touchingMachine.repair(repairSpeed * Time.deltaTime);
        }
    }
}
