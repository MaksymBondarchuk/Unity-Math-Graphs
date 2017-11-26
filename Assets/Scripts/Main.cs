using System;
using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    //private ArrayList myNodes;
    public Camera MainCamera { get; set; }
    //public Camera MainCamera2 { get; set; }
    public GameObject Sphere { get; set; }

    private Vector3 RightClick { get; set; }

    private bool IsRightClicked { get; set; }

    private Vector3 MiddleClick { get; set; }

    private bool IsMiddleClicked { get; set; }

    // ReSharper disable once UnusedMember.Local
    private void Start()
    {
        Cursor.visible = true;
        Debug.Log(string.Format("I am alive! - {0}", DateTime.Now));
        //myNodes = new ArrayList();
    }

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseDown();

        #region MoveCamera
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Pressed right click.");
            RightClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            IsRightClicked = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            //Debug.Log("Released right click.");
            IsRightClicked = false;
        }

        if (IsRightClicked)
            MoveRightPressed();

        if (Input.GetMouseButtonDown(2))
        {
            //Debug.Log("Pressed middle click.");
            MiddleClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            IsMiddleClicked = true;
        }

        if (Input.GetMouseButtonUp(2))
        {
            //Debug.Log("Released middle click.");
            IsMiddleClicked = false;
        }

        if (IsMiddleClicked)
            MoveMiddlePressed();
        #endregion MoveCamera

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            MoveCamera(1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            MoveCamera(-1);
    }

    void OnMouseDown()
    {
        //Debug.Log(string.Format("{0}, {1}, {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mousePosition.z = 10;
        var objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        sphere.transform.position = objPosition;
        //sphere.GetComponent<Renderer>().material.color = Color.red;
        //Debug.Log(string.Format("{0}, {1}, {2}", objPosition.x, objPosition.y, objPosition.z));

        //myNodes.Add(sphere);
    }

    private void MoveCamera(int step)
    {
        var pos = transform.position;
        pos.z += step;
        transform.position = pos;
    }

    private void MoveRightPressed()
    {
        const float velocity = (float)0.001;

        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        var pos = transform.rotation;
        //Debug.Log(string.Format("Rot {0}, {1}, {2}", pos.x, pos.y, pos.z));
        pos.x += velocity * (RightClick.y - mousePosition.y);
        //pos.x += (float)0.01;
        pos.y -= velocity * (RightClick.x - mousePosition.x);
        transform.rotation = pos;
        RightClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        //transform.RotateAround(mousePosition, Vector3.up, Input.GetAxis("Mouse X") * velocity);
    }

    private void MoveMiddlePressed()
    {
        const float velocity = (float) 0.1;
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        var pos = transform.position;
        pos.x += velocity * (MiddleClick.x - mousePosition.x);
        pos.y += velocity * (MiddleClick.y - mousePosition.y);
        transform.position = pos;
        MiddleClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }
}
