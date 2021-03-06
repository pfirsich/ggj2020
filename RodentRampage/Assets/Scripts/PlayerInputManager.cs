﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;

public class PlayerInputManager : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    void SetKeyboardInput(PlayerInput input)
    {
        InputUser.PerformPairingWithDevice(Keyboard.current, input.user);
        string name = input.gameObject.name;
        if (name.StartsWith("Mechanic"))
        {
            string mechanicId = name.Substring(name.Length - 1);
            switch (mechanicId)
            {
                case "A":
                    input.user.ActivateControlScheme("Keyboard_TFGH");
                    break;
                case "B":
                    input.user.ActivateControlScheme("Keyboard_IJKL");
                    break;
                case "C":
                    input.user.ActivateControlScheme("Keyboard_WASD");
                    break;
            }
        }
        else
        {
            input.user.ActivateControlScheme("Keyboard");
        }
    }

    void SetGamepadInput(PlayerInput input, int index)
    {
        InputUser.PerformPairingWithDevice(Gamepad.all[index], input.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        input.user.ActivateControlScheme("Gamepad");
    }

    void OnPlayerJoined(PlayerInput input)
    {
        int gamepadCount = Math.Min(4, Gamepad.all.Count);
        string name = input.gameObject.name;
        string[] gpQueue = {"Mouse", "MechanicA", "MechanicB", "MechanicC"};
        int gpQueueIndex = Array.IndexOf(gpQueue, name);
        if (gamepadCount > gpQueueIndex)
        {
            SetGamepadInput(input, gpQueueIndex);
        }
        else
        {
            SetKeyboardInput(input);
        }
    }
}
