using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechanicComponent : MonoBehaviour
{
    public ToolComponent pickedUpTool;

    private PlayerInput input;
    private ToolComponent touchingTool;

    void Start()
    {
        input = GetComponent<PlayerInput>();        
    }

    void OnThrow(InputValue inputValue)
    {
        if(!pickedUpTool && touchingTool && !touchingTool.pickedUpBy)
        {
            pickedUpTool = touchingTool;
            touchingTool.pickedUpBy = this;
            touchingTool.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            touchingTool.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
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
