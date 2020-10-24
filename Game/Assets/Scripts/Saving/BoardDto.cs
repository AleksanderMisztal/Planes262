using System;
using UnityEngine;

namespace Planes262.Saving
{    
    [Serializable]
    public class BoardDto
    {
        public string background;
        public Vector3 offset;
        public int xSize;
        public int ySize;
        public float gridSize;
    }
}