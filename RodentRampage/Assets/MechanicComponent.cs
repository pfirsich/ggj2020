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

    public float currentTime;
    public AudioSource repairSound;
    public AudioSource wrongToolSound;
    public float repairIntervall = 0.6f;
    public float lastRepair= 0.0f;
    public float timeSinceLastRepair;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        wrongToolSound = GetComponent<AudioSource>();
    }

    private bool fuckoff;

    void OnThrow(InputValue inputValue)
    {
        throwing = inputValue.Get<float>() > 0.5;

        if (throwing)
        {
            if (!pickedUpTool && touchingTool && !touchingTool.pickedUpBy)
            {
                pickedUpTool = touchingTool;
                touchingTool.pickedUpBy = this;
                repairSound = pickedUpTool.GetComponent<AudioSource>();

                var hand = transform.Find("m_mechanic/Armature/Body/right Shoulder/right Shoulder.001/right Hand 1");
                pickedUpTool.gameObject.transform.parent = hand;
                var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.detectCollisions = false;
                pickedUpTool.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                pickedUpTool.gameObject.transform.localRotation = new Quaternion();

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
                var strength = Mathf.Lerp(minThrowStrength, 1.0f, throwStrength);

                pickedUpTool.gameObject.transform.parent = null;
                var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.detectCollisions = true;
                rb.velocity = strength * throwVelocity * throwDirection;
                Physics.IgnoreCollision(GetComponent<Collider>(), pickedUpTool.GetComponent<Collider>(), true);
                ignoredTools.Add(pickedUpTool.GetComponent<Collider>());
                pickedUpTool.pickedUpBy = null;
                pickedUpTool = null;
            }
        }
    }

    void OnUse(InputValue value)
    {
        repairing = value.Get<float>() > 0.5f;
        if (isRepairing())
        {
            repairSound.Play(0);
            lastRepair= Time.time;
        }
        else
        {
            if (couldRepair() && !rightTool())
                wrongToolSound.Play();
        }
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

    public bool isRepairing()
    {
        return couldRepair() && rightTool();
    }

    public bool couldRepair()
    {
        if (!repairing)
            return false;
        if (!touchingMachine)
            return false;
        if (touchingMachine.damageTypes.Count == 0)
            return false;
        return true;
    }

    public bool rightTool()
    {
        return pickedUpTool.toolType == touchingMachine.damageTypes[0];
    }

    public bool isThrowing()
    {
        return throwing && !pickingUp;
    }

    void Update()
    {
        if (isRepairing())
        {
            touchingMachine.repair(repairSpeed * Time.deltaTime);
            currentTime = Time.time;
            timeSinceLastRepair = currentTime - lastRepair;
            if (timeSinceLastRepair > repairIntervall)
            {
                Debug.Log("Time " + currentTime);
                repairSound.Play(0);
                lastRepair = currentTime;
            }
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

        if(fuckoff)
        {
            fuckoff = false;
                            var moveDir = GetComponent<PlayerMovement>().getMoveDir();
                if (moveDir.magnitude < 1e-5)
                    moveDir = transform.rotation * Vector3.forward;
                var throwDirection = new Vector3(moveDir.x, throwYDir, moveDir.z);
                var strength = Mathf.Lerp(minThrowStrength, 1.0f, throwStrength);
                var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
                rb.velocity = strength * throwVelocity * throwDirection;
        }
    }
}
