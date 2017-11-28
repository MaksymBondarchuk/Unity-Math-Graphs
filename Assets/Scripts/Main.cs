using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private List<Transform> HitObject { get; set; }

    // ReSharper disable once UnusedMember.Local
    void Start()
    {
        HitObject = new List<Transform>();
    }

    // ReSharper disable once UnusedMember.Local
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            AddNode();

        foreach (var objectHit in HitObject)
            objectHit.GetComponent<Renderer>().material.color = Color.gray;
        HitObject.Clear();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            objectHit.GetComponent<Renderer>().material.color = Color.blue;
            HitObject.Add(objectHit);
        }
    }


    // ReSharper disable once UnusedMember.Local
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //        AddNode();
    //}

    void AddNode()
    {
        //Debug.Log(string.Format("{0}, {1}, {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mousePosition.z = 10;
        var objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        sphere.transform.position = objPosition;
        sphere.GetComponent<Renderer>().material.color = Color.red;
    }
}
