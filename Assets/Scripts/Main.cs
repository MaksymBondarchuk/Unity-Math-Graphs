using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UnusedMember.Local

namespace Assets.Scripts
{
    // ReSharper disable once UnusedMember.Global
    public class Main : MonoBehaviour
    {
        private List<Transform> HitObject { get; set; }
        private Transform FirstNode { get; set; }

        private bool IsInAddVertexMode { get; set; }
        private bool IsInAddEdgeMode { get; set; }

        private void Start()
        {
            HitObject = new List<Transform>();
        }

        private void Update()
        {
            Transform objectHit = null;

            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.L) && IsInAddVertexMode)
                AddNode();

            foreach (var objectHited in HitObject)
                objectHited.GetComponent<Renderer>().material.color = Color.gray;
            HitObject.Clear();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                objectHit = hit.transform;
                objectHit.GetComponent<Renderer>().material.color = Color.blue;
                HitObject.Add(objectHit);
            }

            if (Input.GetMouseButtonDown(0) && (IsInAddEdgeMode || Input.GetKey(KeyCode.L)) && objectHit != null)
            {
                if (FirstNode == null)
                    FirstNode = objectHit;
                else
                {
                    OnDrawGizmosSelected(FirstNode, objectHit);
                    FirstNode = null;
                }
            }

            //if (Input.GetButtonDown("Add vertex"))
            //    Debug.Break();

        }


        // ReSharper disable once UnusedMember.Local
        //private void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //        AddNode();
        //}

        private static void AddNode()
        {
            //Debug.Log(string.Format("{0}, {1}, {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mousePosition.z = 10;
            var objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            sphere.transform.position = objPosition;
            sphere.GetComponent<Renderer>().material.color = Color.red;
        }

        private static void OnDrawGizmosSelected(Transform node1, Transform node2)
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            const float radius = (float).1;
            cylinder.transform.localScale = new Vector3(radius, Vector3.Distance(node2.position, node1.position) / 2, radius);


            cylinder.transform.position = node1.position + (node2.position - node1.position) / 2;
            cylinder.transform.up = node2.position - node1.position;
        }

        public void OnAddVertexButtonclick()
        {
            IsInAddVertexMode = !IsInAddVertexMode;

            //ColorBlock colorBlock = new ColorBlock();
            //colorBlock.normalColor = Color.red;

            //var buttons = FindObjectsOfType<Button>();// GetComponent<Canvas>().GetComponents<Button>();
            //var colorBlock = buttons[0].colors;
            //colorBlock.normalColor = Color.red;
            //buttons[0].colors = colorBlock;
        }

        public void OnAddEdgeButtonclick()
        {
            IsInAddEdgeMode = !IsInAddEdgeMode;
        }
    }
}
