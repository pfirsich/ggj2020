using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechanicComponent : MonoBehaviour
{
    public float throwYDir = 0.2f;
    public float throwVelocity = 10.0f;
    public float throwOffsetLen = 2.0f;

    public ToolComponent pickedUpTool;

    private PlayerInput input;
    private ToolComponent touchingTool;

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
            var throwDirection = new Vector3(moveDir.x, throwYDir, moveDir.z);

            var rb = pickedUpTool.gameObject.GetComponent<Rigidbody>();
            rb.velocity = throwVelocity * throwDirection;
            rb.position += throwOffsetLen * new Vector3(moveDir.x, 0.0f, moveDir.y);

            rb.isKinematic = false;
            rb.detectCollisions = true;

            Debug.Log("pickedUpTool: "+ pickedUpTool);
            pickedUpTool.pickedUpBy = null;

            pickedUpTool = null;
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
        var tool = other.GetComponent<ToolComponent>();
        if (tool)
        {
            touchingTool = tool;
            
        }
    }

    void onTriggerExit(Collider other)
    {
        var tool = other.GetComponent<ToolComponent>();
        if (tool && touchingTool == tool)
        {
            touchingTool = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
