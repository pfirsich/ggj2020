using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechanicComponent : MonoBehaviour
{
    public float throwYDir = 0.0f;
    public float throwVelocity = 20.0f;
    public float throwOffsetLen = 4.0f;
    public float repairSpeed = 0.7f;
    public float maxThrowChargeTime = 1.0f;
    public float minThrowStrength = 0.3f;

    public ToolComponent pickedUpTool;

    private PlayerInput input;
    private ToolComponent touchingTool;
    private MachineComponent touchingMachine;
    private bool repairing;
    private bool throwing;
    public float throwStrength;
    private bool pickingUp;
    private List<Collider> ignoredTools = new List<Collider>();

    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void OnThrow(InputValue inputValue)
    {
        throwing = inputValue.Get<float>() > 0.5;

        if (throwing)
        {
            if (!pickedUpTool && touchingTool && !touchingTool.pickedUpBy)
            {
                pickedUpTool = touchingTool;
                touchingTool.pickedUpBy = this;

                var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
                Physics.IgnoreCollision(GetComponent<Collider>(), pickedUpTool.GetComponent<Collider>(), true);
                ignoredTools.Add(pickedUpTool.GetComponent<Collider>());
                pickedUpTool.gameObject.transform.position = transform.position;

                pickingUp = true;
            }
            else
            {
                throwStrength = 0.0f;

                pickingUp = false;
            }
        }
        else
        {
            if (pickedUpTool && !pickingUp)
            {
                var moveDir = GetComponent<PlayerMovement>().getMoveDir();
                if (moveDir.magnitude < 1e-5)
                    moveDir = transform.rotation * Vector3.forward;
                var throwDirection = new Vector3(moveDir.x, throwYDir, moveDir.z);

                var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
                var strength = Mathf.Lerp(minThrowStrength, 1.0f, throwStrength);
                rb.velocity = strength * throwVelocity * throwDirection;
                //rb.position += throwOffsetLen * new Vector3(moveDir.x, 0.0f, moveDir.y);

                pickedUpTool.pickedUpBy = null;
                pickedUpTool = null;
            }
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

        if (other.transform.parent)
        {
            var machine = other.transform.parent.GetComponent<MachineComponent>();
            if (machine)
            {
                touchingMachine = machine;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        var tool = other.GetComponent<ToolComponent>();
        if (tool && touchingTool == tool)
        {
            touchingTool = null;
        }

        if (other.transform.parent)
        {
            var machine = other.transform.parent.GetComponent<MachineComponent>();
            if (machine && touchingMachine == machine)
            {
                touchingMachine = null;
            }
        }
    }

    void Update()
    {
        if (touchingMachine && repairing)
        {
            touchingMachine.repair(repairSpeed * Time.deltaTime);
        }

        if (throwing)
            throwStrength = Mathf.Min(1.0f, throwStrength + Time.deltaTime / maxThrowChargeTime);

        for (int i = ignoredTools.Count - 1; i >= 0; --i)
        {
            var dist = (transform.position - ignoredTools[i].gameObject.transform.position).magnitude;
            if (dist > 2.2f) {
                Physics.IgnoreCollision(GetComponent<Collider>(), ignoredTools[i], false);
                ignoredTools.RemoveAt(i);
            }
        }
    }
}
