using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class JsonObject
    {
        public List<JsonVertex> Vertexes;
        public List<JsonEdge> Edges;


        //public List<Vector3> Vertexes;
        //public Vector3[] Vertexes;
        ////public List<Vector3> Edges;
        //public Vector3[] Edges;
    }

    [Serializable]
    public class JsonVertex
    {
        public float X;
        public float Y;
        public float Z;
        //public Vector3 Vertex;
    }

    [Serializable]
    public class JsonEdge
    {
        public float X1;
        public float Y1;
        public float Z1;

        public float X2;
        public float Y2;
        public float Z2;
        //public float x;
        //public Vector3 Edge;
    }
}
