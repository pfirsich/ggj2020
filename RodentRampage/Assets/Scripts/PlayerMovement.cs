using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    const float MAX_MOVE_SPEED = 5.0f;
    const float ACCEL = 20.0f;
    const float FRICTION = 20.0f;
    const float REVERSE_ACCEL_FACTOR = 2.0f;

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
        Vector3 accel = ACCEL * new Vector3(moveDir.x, 0.0f, moveDir.y).normalized;
        if (accel.magnitude < 1.0e-5)
        {
            vel -= FRICTION * vel * dt;
        }
        else
        {
            if (accel.x * vel.x < 0.0f)
                accel.x *= REVERSE_ACCEL_FACTOR;
            if (accel.y * vel.y < 0.0f)
                accel.y *= REVERSE_ACCEL_FACTOR;

            vel += dt * accel;
            if (vel.magnitude > MAX_MOVE_SPEED)
            {
                vel *= MAX_MOVE_SPEED / vel.magnitude;
            }
        }
        body.velocity = vel;
    }

    void Update()
    {

    }
}
