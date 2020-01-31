using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const float MAX_MOVE_SPEED = 5.0f;
    const float ACCEL = 20.0f;
    const float FRICTION = 20.0f;
    const float REVERSE_ACCEL_FACTOR = 2.0f;

    private Rigidbody body;

    float bool2float(bool b)
    {
        return b ? 1.0f : 0.0f;
    }

    Vector3 getMovementVector()
    {
        return new Vector3(
            bool2float(Input.GetKey(KeyCode.D)) - bool2float(Input.GetKey(KeyCode.A)),
            0.0f,
            bool2float(Input.GetKey(KeyCode.W)) - bool2float(Input.GetKey(KeyCode.S))).
            normalized;
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 vel = body.velocity;
        Vector3 accel = ACCEL * getMovementVector();
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
