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

    public float currentTime;
    AudioSource gnawSound;
    public float gnawIntervall = 0.6f;
    public float lastGnaw = 0.0f;
    public float timeSinceLastGnaw;



    void Start()
    {
        input = GetComponent<PlayerInput>();
        gnawSound = GetComponent<AudioSource>();
        currentTime = Time.time;
        lastGnaw = 0.0f ;
    }

    void Update()
    {
        if (touchingMachine && gnawing) {
            touchingMachine.gnaw(gnawSpeed * Time.deltaTime);

            currentTime = Time.time;
            timeSinceLastGnaw = currentTime - lastGnaw;
            if (timeSinceLastGnaw > gnawIntervall)
            {
                gnawSound.Play(0);
                lastGnaw = currentTime;
            }
        }

    }

    void OnUse(InputValue value)
    {
        gnawing = value.Get<float>() > 0.5f;
        //hier den sound nach nem zeit intervall Time.time
        //TODO check if at machine
        if (touchingMachine && gnawing)
        {
            gnawSound.Play(0);
            lastGnaw = Time.time;
        }
    }

    void OnTriggerEnter(Collider other)
    {
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
        if (other.transform.parent)
        {
            var machine = other.transform.parent.GetComponent<MachineComponent>();
            if (machine && touchingMachine == machine)
            {
                touchingMachine = null;
            }
        }
    }
}
