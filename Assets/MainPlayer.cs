using System;
using UnityEngine;
using System.Collections;

public class MainPlayer : MonoBehaviour
{
    private ArrayList myNodes;

    // Use this for initialization
    void Start()
    {
        Debug.Log(string.Format("I am alive! - {0}", DateTime.Now));
        myNodes = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseDown();
        //Debug.Log("Pressed left click.");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed right click.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
    }

    void OnMouseDown()
    {
        Debug.Log(string.Format("{0}, {1}, {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = mousePosition;

        myNodes.Add(sphere);
    }
}
