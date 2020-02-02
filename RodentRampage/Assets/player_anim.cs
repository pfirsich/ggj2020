using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_anim : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody body;
    private Animator anim;
    void Start()
    {
        body = transform.parent.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetFloat("CycleOffset", Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if(body.velocity.magnitude > 0.1)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        var mechanic = transform.parent.GetComponent<MechanicComponent>();
        anim.SetBool("IsRepairing", mechanic.isRepairing());
        anim.SetBool("IsThrowing", mechanic.isThrowing());
    }
}
