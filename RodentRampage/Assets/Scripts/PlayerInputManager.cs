using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;

public class PlayerInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
        InputUser.PerformPairingWithDevice(Gamepad.all[index], input.user);
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

        /*switch (gamepadCount)
        {
            case 0:
                // SetKeyboardInput for all
                break;
            case 1:
                // SetGamepad(0) für Mouse, sonst SetKeyboard
                break;
            case 2:
                // SetGamepad(0) für Mouse, SetGamepad(1) für A, sonst SetKeyboard
                break;
            case 3:
                // SetGamepad(0) für Mouse, SetGamepad(1) für A, SetGamepad(2) für B, SetKeyboard für C
                break;
            case 4:
                // siehe oben, aber SetGamepad(3) für C
                break;
        }*/
    }
}
