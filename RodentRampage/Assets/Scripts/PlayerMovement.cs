using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float maxMoveSpeed = 5.0f;
    public float acceleration = 20.0f;
    public float friction = 20.0f;
    public float reverseFrictionAccel = 2.0f;


    private Rigidbody body;
    private PlayerInput input;

    private Vector2 moveDir;

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();

    }

    public Vector3 getMoveDir()
    {
        var dir = new Vector3(moveDir.x, 0.0f, moveDir.y);
        if (dir.magnitude < 0.2)
            return Vector3.zero;
        if (dir.magnitude > 1.0)
            dir /= dir.magnitude;
        return dir;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 vel = body.velocity;
        Vector3 accel = acceleration * getMoveDir();
        if (accel.magnitude < 1.0e-5)
        {
            vel -= friction * vel * dt;
        }
        else
        {
            if (accel.x * vel.x < 0.0f)
                accel.x *= reverseFrictionAccel;
            if (accel.z * vel.z < 0.0f)
                accel.z *= reverseFrictionAccel;

            vel += dt * accel;
            if (vel.magnitude > maxMoveSpeed)
            {
                vel *= maxMoveSpeed / vel.magnitude;
            }
        }

        vel.y = 0; // keep only the horizontal direction
        body.velocity = vel;
        //var lookDir = body.position - transform.position;
        if (vel.sqrMagnitude > 0.1)
        {
            transform.rotation = Quaternion.LookRotation(vel);
        }
    }

    void Update()
    {
    }
}
