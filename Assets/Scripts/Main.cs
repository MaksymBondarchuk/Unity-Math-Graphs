using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
// ReSharper disable UnusedMember.Global

// ReSharper disable UnusedMember.Local

namespace Assets.Scripts
{
    // ReSharper disable once UnusedMember.Global
    [RequireComponent(typeof(AudioSource))]
    public class Main : MonoBehaviour
    {
        private List<Transform> HitObject { get; set; }
        private Transform FirstNode { get; set; }

        private bool IsInAddVertexMode { get; set; }
        private bool IsInAddEdgeMode { get; set; }
        private bool IsInDeleteMode { get; set; }
        private bool IsInOrientedMode { get; set; }


        private List<JsonVertex> JsonVertices { get; set; }
        private List<JsonEdge> JsonEdges { get; set; }

        private void Start()
        {
            HitObject = new List<Transform>();
            JsonVertices = new List<JsonVertex>();
            JsonEdges = new List<JsonEdge>();

            //var smoke = GameObject.Find("WhiteSmoke");
            //smoke.layer = 30;
            //smoke.SetActive(false);
        }

        private void Update()
        {
            Transform objectHit = null;

            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.L) && IsInAddVertexMode)
                AddVertex(Input.mousePosition);


            foreach (var objectHited in HitObject)
                if (objectHited != null)
                    objectHited.GetComponent<Renderer>().material.color = Color.gray;
            HitObject.Clear();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                objectHit = hit.transform;
                objectHit.GetComponent<Renderer>().material.color = Color.yellow;
                //objectHit.GetComponent<Renderer>().material.color = new Vector4(0, 209, 174, 45);
                //objectHit.GetComponent<Renderer>().material.color = new Vector4(0, 255, 0, 0);
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

            if (Input.GetMouseButtonDown(0) && IsInDeleteMode)
                Delete(objectHit);
        }

        private void AddVertex(Vector3 center, bool isStrongCenter = false)
        {
            IsInAddVertexMode = false;

            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            var pos = center;
            if (isStrongCenter)
                sphere.transform.position = center;
            else
            {
                center.z = 10;
                var objPosition = Camera.main.ScreenToWorldPoint(center);
                sphere.transform.position = objPosition;
                pos = objPosition;
            }

            //sphere.GetComponent<Renderer>().material.color = Color.red;
            sphere.tag = "vertex";

            JsonVertices.Add(new JsonVertex
            {
                X = sphere.transform.position.x,
                Y = sphere.transform.position.y,
                Z = sphere.transform.position.z
            });

            var smoke = GameObject.Find("WhiteSmoke");
            //smoke.SetActive(true);
            var newSmoke = (GameObject)Instantiate(smoke, pos, new Quaternion(-90, 0, 0, 90));
            //smoke.SetActive(false);
            newSmoke.SetActive(true);

            var sound = FindObjectsOfType<AudioSource>().First(s => s.tag == "vertex sound");
            sound.Play();
        }

        private void AddEdge(Vector3 node1, Vector3 node2)
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            const float radius = (float).1;
            cylinder.transform.localScale = new Vector3(radius, Vector3.Distance(node2, node1) / 2, radius);


            cylinder.transform.position = node1 + (node2 - node1) / 2;
            cylinder.transform.up = node2 - node1;
            cylinder.tag = "edge";

            #region Arrow
            if (IsInOrientedMode)
            {
                const float arrowLength = 1;

                var arrow1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                arrow1.transform.localScale = new Vector3(radius, arrowLength, radius);
                arrow1.transform.position = node2;
                arrow1.transform.up = node2 - node1 + Vector3.up;
                arrow1.transform.Translate(Vector3.down);

                var arrow2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                arrow2.transform.localScale = new Vector3(radius, arrowLength, radius);
                arrow2.transform.position = node2;
                arrow2.transform.up = node2 - node1 - Vector3.up;
                arrow2.transform.Translate(Vector3.down);
            }

            #endregion

            JsonEdges.Add(new JsonEdge
            {
                X1 = node1.x,
                Y1 = node1.y,
                Z1 = node1.z,
                X2 = node2.x,
                Y2 = node2.y,
                Z2 = node2.z
            });

            var sound = FindObjectsOfType<AudioSource>().First(s => s.tag == "edge sound");
            sound.Play();

            IsInAddEdgeMode = false;
        }

        public void OnAddVertexButtonclick()
        {
            //var buttons = FindObjectsOfType<Button>();
            //buttons[buttons.Length - 1].GetComponent<Image>().color = !IsInAddVertexMode ? Color.red : Color.white;
            IsInAddVertexMode = true;
        }

        public void OnAddEdgeButtonclick()
        {
            //var buttons = FindObjectsOfType<Button>();
            //buttons[0].GetComponent<Image>().color = !IsInAddEdgeMode ? Color.red : Color.white;

            IsInAddEdgeMode = true;
        }

        public void OnDeleteButtonclick()
        {
            //var buttons = FindObjectsOfType<Button>();
            //buttons[0].GetComponent<Image>().color = !IsInAddEdgeMode ? Color.red : Color.white;

            IsInDeleteMode = true;
        }

        public void OnExportButtonclick()
        {
            // ReSharper disable once RedundantAssignment
            var path = "C:\\3D Graph Builder\\GraphFile.json";

#if UNITY_EDITOR
            path = EditorUtility.SaveFilePanel(
                    "Select file to save",
                    "JavaScript Object Notation",
                    "TestFile" + ".json",
                    "JSON");
#endif


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

            var sound = FindObjectsOfType<AudioSource>().First(s => s.tag == "applause");
            sound.Play();
        }

        public void OnImportButtonclick()
        {
            // ReSharper disable once RedundantAssignment
            var path = "C:\\3D Graph Builder\\GraphFile.json";
#if UNITY_EDITOR
            path = EditorUtility.OpenFilePanel(
                    "Select file to save",
                    "",
                    "JSON");
#endif

            if (path.Length != 0)
            {
                //DeleteAll();
                var delete =
                    FindObjectsOfType<GameObject>().Where(o => o.tag == "vertex" || o.tag == "edge" || o.tag == "smoke");
                var smoke = GameObject.Find("WhiteSmoke");

                using (var file = new StreamReader(path))
                {
                    var json = file.ReadToEnd();
                    var obj = JsonUtility.FromJson<JsonObject>(json);
                    smoke.transform.position = new Vector3(obj.Vertexes[0].X, obj.Vertexes[0].Y, obj.Vertexes[0].Z);
                    foreach (var vertex in obj.Vertexes)
                        AddVertex(new Vector3(vertex.X, vertex.Y, vertex.Z), true);
                    foreach (var edge in obj.Edges)
                        AddEdge(new Vector3(edge.X1, edge.Y1, edge.Z1), new Vector3(edge.X2, edge.Y2, edge.Z2));
                }
                foreach (var o in delete)
                    Destroy(o);
            }

            var sound = FindObjectsOfType<AudioSource>().First(s => s.tag == "applause");
            sound.Play();
        }

        public void OnOrientedEdgeToggleChange()
        {
            IsInOrientedMode = !IsInOrientedMode;
        }

        private static void DeleteAll()
        {
            foreach (var o in FindObjectsOfType<GameObject>().Where(o => o.tag == "vertex" || o.tag == "edge" || o.tag == "smoke"))
                Destroy(o);
        }

        private void Delete(Transform objectHit)
        {
            IsInDeleteMode = false;

            var vertex = FindObjectsOfType<GameObject>().FirstOrDefault(o => o.transform == objectHit);
            var edges = FindObjectsOfType<GameObject>().Where(o => vertex != null && o.tag == "edge" && o.transform.position == vertex.transform.position);

            var smoke = FindObjectsOfType<GameObject>().FirstOrDefault(o => vertex != null && o.tag == "smoke" && o.transform.position == vertex.transform.position);

            Destroy(vertex);
            Destroy(smoke);
            foreach (var edge in edges)
                Destroy(edge);
        }
    }
}
