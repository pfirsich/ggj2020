using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolComponent : MonoBehaviour
{
    public enum ToolType { Hammer, Wrench, Screwdriver, Drill, Oilcan };
    public ToolType toolType;
    public MechanicComponent pickedUpBy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUpBy)
        {
            transform.localPosition = pickedUpBy.transform.localPosition;
        }
    }
}
