using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getIcon : MonoBehaviour
{
    public Sprite Wrench;
    public Sprite Hammer;
    public Sprite Oilcan;
    public Sprite ScrewDriver;
    public Sprite Drillcon;
    public int ListPosition;
    private List<ToolType> tools;
    private ToolType tool;
    private MachineComponent machine;


    // Start is called before the first frame update
    void Start()
    {
        machine = transform.parent.transform.parent.GetComponent<MachineComponent>();

        //a = Resources.Load<Sprite>("ScrewDriverIcon");
    }

    // Update is called once per frame
    void Update()
    {
        if (machine.damageTypes.Count > ListPosition)
        {
            tool = machine.damageTypes[ListPosition];
        

        switch (tool)
        {
            case ToolType.Hammer:
                gameObject.GetComponent<Image>().sprite = Hammer;
                break;
            case ToolType.Wrench:
                gameObject.GetComponent<Image>().sprite = Wrench;
                break;
            case ToolType.Screwdriver:
                gameObject.GetComponent<Image>().sprite = ScrewDriver;
                break;
            case ToolType.Drill:
                gameObject.GetComponent<Image>().sprite = Drillcon;
                break;
            case ToolType.Oilcan:
                gameObject.GetComponent<Image>().sprite = Oilcan;
                break;
            default:
                gameObject.GetComponent<Image>().sprite = null;
                break;
        }
        }
    }
}
