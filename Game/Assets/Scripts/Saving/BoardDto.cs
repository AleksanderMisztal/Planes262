using System;
using UnityEngine;

namespace Planes262.Saving
{    
    [Serializable]
    public class BoardDto
    {
        public string background;
        public int xSize;
        public int ySize;
        public Vector3 offset;
        public float ortoSize;
    }
}