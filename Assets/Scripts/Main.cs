using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable UnusedMember.Global

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

        private List<JsonVertex> JsonVertices { get; set; }
        private List<JsonEdge> JsonEdges { get; set; }

        private void Start()
        {
            HitObject = new List<Transform>();
            JsonVertices = new List<JsonVertex>();
            JsonEdges = new List<JsonEdge>();
        }

        private void Update()
        {
            Transform objectHit = null;

            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.L) && (IsInAddVertexMode || Input.GetKey(KeyCode.A)))
                AddVertex(Input.mousePosition);

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
                    AddEdge(FirstNode.position, objectHit.position);
                    FirstNode = null;
                }
            }
        }

        private void AddVertex(Vector3 center, bool isStrongCenter = false)
        {
            //Debug.Log(string.Format("{0}, {1}, {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            //var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            if (isStrongCenter)
            {
                sphere.transform.position = center;
            }
            else
            {
                center.z = 10;
                var objPosition = Camera.main.ScreenToWorldPoint(center);
                sphere.transform.position = objPosition;
            }

            //sphere.GetComponent<Renderer>().material.color = Color.red;
            sphere.tag = "vertex";

            JsonVertices.Add(new JsonVertex
            {
                X = sphere.transform.position.x,
                Y = sphere.transform.position.y,
                Z = sphere.transform.position.z
            });
        }

        private void AddEdge(Vector3 node1, Vector3 node2)
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            const float radius = (float).1;
            cylinder.transform.localScale = new Vector3(radius, Vector3.Distance(node2, node1) / 2, radius);


            cylinder.transform.position = node1 + (node2 - node1) / 2;
            cylinder.transform.up = node2 - node1;
            cylinder.tag = "edge";

            JsonEdges.Add(new JsonEdge
            {
                X1 = node1.x,
                Y1 = node1.y,
                Z1 = node1.z,
                X2 = node2.x,
                Y2 = node2.y,
                Z2 = node2.z
            });
        }

        public void OnAddVertexButtonclick()
        {
            var buttons = FindObjectsOfType<Button>();
            buttons[2].GetComponent<Image>().color = !IsInAddVertexMode ? Color.red : Color.white;
            IsInAddVertexMode = !IsInAddVertexMode;
        }

        public void OnAddEdgeButtonclick()
        {
            var buttons = FindObjectsOfType<Button>();
            buttons[0].GetComponent<Image>().color = !IsInAddEdgeMode ? Color.red : Color.white;

            IsInAddEdgeMode = !IsInAddEdgeMode;
        }

        public void OnExportButtonclick()
        {
            var path = EditorUtility.SaveFilePanel(
                 "Select file to save",
                 "JavaScript Object Notation",
                 "TestFile" + ".json",
                 "JSON");

            if (path.Length != 0)
            {
                var jsonObject = new JsonObject
                {
                    Vertexes = JsonVertices,
                    Edges = JsonEdges,
                };

                var json = JsonUtility.ToJson(jsonObject);
                using (var file = new StreamWriter(path))
                    file.WriteLine(json);
            }
        }

        public void OnImportButtonclick()
        {
            var path = EditorUtility.OpenFilePanel(
                 "Select file to save",
                 "",
                 "JSON");

            if (path.Length != 0)
            {
                DeleteAll();
                using (var file = new StreamReader(path))
                {
                    var json = file.ReadToEnd();
                    var obj = JsonUtility.FromJson<JsonObject>(json);
                    foreach (var vertex in obj.Vertexes)
                        AddVertex(new Vector3(vertex.X, vertex.Y, vertex.Z), true);
                    foreach (var edge in obj.Edges)
                        AddEdge(new Vector3(edge.X1, edge.Y1, edge.Z1), new Vector3(edge.X2, edge.Y2, edge.Z2));
                }
            }
        }

        public void DeleteAll()
        {
            foreach (var o in FindObjectsOfType<GameObject>().Where(o => o.tag == "vertex" || o.tag == "edge"))
                Destroy(o);
        }
    }
}
