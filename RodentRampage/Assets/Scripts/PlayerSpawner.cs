using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        string[] names = Input.GetJoystickNames();
        Debug.Assert(names.Length > 0);

        if (names.Length >= 4) {
            // assign mouse and all players to gamepads
        }
        else
        {
            // assign keyboard to mouse
            // assign all other gamepads to players
        }
    }
}
