using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechanicComponent : MonoBehaviour
{
    public GameObject attachedTool;
    private PlayerInput input;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
