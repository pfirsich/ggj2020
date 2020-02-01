using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPickUp : MonoBehaviour
{
    private float speed = 2f;
    Renderer rend;
    int colorPicker = 0;

    // Start is called before the first frame update
    private void Start()
    {
        //rend = GetComponent<Renderer>();
        //transform.position = Vector3.zero;
        //transform.rotation = Quaternion.Euler(0, 90, 0);

        //Create two GameObjects to act as walls
        //GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //Move the walls to the correct positions
        //leftWall.transform.position = new Vector3(-10, 0, 0);
        //rightWall.transform.position = new Vector3(10, 0, 0);
        //Scale the walls
        //leftWall.transform.localScale = new Vector3(1, 2, 1);
        //rightWall.transform.localScale = new Vector3(1, 2, 1);
    }

    //moves the Primitive 2 units a second in the forward direction
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

   


    //When the Primitive collides with the walls, it will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        speed = 0;
        colorPicker = Random.Range(0, 10);
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {

    }
}
