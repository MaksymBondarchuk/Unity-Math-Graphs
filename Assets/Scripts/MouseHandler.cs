using System;
using UnityEngine;
using System.Collections;

public class MouseHandler : MonoBehaviour
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
        //myNodes = new ArrayList();
    }

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
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

    private void MoveCamera(int step)
    {
        if (step > 0)
            transform.Translate(Vector3.forward);
        else
            transform.Translate(Vector3.back);
        //var pos = transform.position;
        //pos.z += step;
        //transform.position = pos;
    }

    private void MoveRightPressed()
    {
        const float velocity = (float)0.001;

        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        //var rot = transform.rotation;
        ////Debug.Log(string.Format("Rot {0}, {1}, {2}", pos.x, pos.y, pos.z));
        //rot.x += velocity * (RightClick.y - mousePosition.y);
        ////pos.x += (float)0.01;
        //rot.y -= velocity * (RightClick.x - mousePosition.x);
        //transform.rotation = rot;
        //RightClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        //transform.RotateAround(mousePosition, Vector3.up, Input.GetAxis("Mouse X") * velocity);

        //float tiltAroundZ = Input.GetAxis("Horizontal") * 15;
        //float tiltAroundX = Input.GetAxis("Vertical") * 15;
        //Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 15);

        transform.RotateAround(transform.position, new Vector3(1.0f, 0.0f, 0.0f), (RightClick.y - mousePosition.y) * Time.deltaTime * 5);
        transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), -(RightClick.x - mousePosition.x) * Time.deltaTime * 5);
        transform.RotateAround(transform.position, new Vector3(0.0f, 0.0f, 1.0f), (RightClick.z - mousePosition.z) * Time.deltaTime * 5);
        RightClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }

    private void MoveMiddlePressed()
    {
        const float velocity = (float)0.1;
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        var pos = transform.position;
        pos.x += velocity * (MiddleClick.x - mousePosition.x);
        pos.y += velocity * (MiddleClick.y - mousePosition.y);
        transform.position = pos;
        MiddleClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }
}
