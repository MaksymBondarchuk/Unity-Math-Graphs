using System;
using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    //private ArrayList myNodes;
    public Camera MainCamera { get; set; }
    //public Camera MainCamera2 { get; set; }
    public GameObject Sphere { get; set; }

    // Use this for initialization
    void Start()
    {
        Debug.Log(string.Format("I am alive! - {0}", DateTime.Now));
        //myNodes = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseDown();
        //Debug.Log("Pressed left click.");

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Pressed right click.");
            MoveCamera(1);
        }

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            MoveCamera(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            MoveCamera(-1);
        }

    }

    void OnMouseDown()
    {
        Debug.Log(string.Format("{0}, {1}, {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mousePosition.z = 10;
        var objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        sphere.transform.position = objPosition;
        //sphere.GetComponent<Renderer>().material.color = Color.red;
        Debug.Log(string.Format("{0}, {1}, {2}", objPosition.x, objPosition.y, objPosition.z));

        //myNodes.Add(sphere);
    }

    void MoveCamera(int step)
    {
        var pos = transform.position;
        pos.z += step;
        transform.position = pos;
    }
}
