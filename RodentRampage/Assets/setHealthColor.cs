using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class setHealthColor : MonoBehaviour
{
    //public Image bar;
    // Start is called before the first frame update
    private Image bar;
    private MachineComponent machine;
    void Start()
    {
       bar = GetComponent<Image>();
        machine = transform.parent.transform.parent.GetComponent<MachineComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        int broken = machine.getBrokenness();

        switch (broken)
        {
            case 2:
                bar.color = Color.yellow;
                break;
            case 3:
                bar.color = Color.red;
                break;
            default:
                bar.color = Color.green;
                break;
        }

    }
}
