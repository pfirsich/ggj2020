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

    float bool2float(bool b)
    {
        return b ? 1.0f : 0.0f;
    }

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 vel = body.velocity;
        Vector3 accel = acceleration * new Vector3(moveDir.x, 0.0f, moveDir.y).normalized;
        if (accel.magnitude < 1.0e-5)
        {
            vel -= friction * vel * dt;
        }
        else
        {
            if (accel.x * vel.x < 0.0f)
                accel.x *= reverseFrictionAccel;
            if (accel.y * vel.y < 0.0f)
                accel.y *= reverseFrictionAccel;

            vel += dt * accel;
            if (vel.magnitude > maxMoveSpeed)
            {
                vel *= maxMoveSpeed / vel.magnitude;
            }
        }
        body.velocity = vel;
    }

    void Update()
    {

    }
}
