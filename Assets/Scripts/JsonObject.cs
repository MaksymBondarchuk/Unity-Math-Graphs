using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class JsonObject
    {
        public List<JsonVertex> Vertices;
        public List<JsonEdge> Edges;
    }

    [Serializable]
    public class JsonVertex
    {
        public float X;
        public float Y;
        public float Z;
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


        public bool IsOriented;
        public int Weight;
    }
}
