using System;
using GameDataStructures.Positioning;

namespace GameDataStructures
{
    [Serializable]
    public class Board
    {
        public readonly int xSize;
        public readonly int ySize;

        public VectorTwo Center { get; }

        public Board(int xSize, int ySize)
        {
            this.xSize = xSize;
            this.ySize = ySize;

            Center = new VectorTwo(xSize / 2, ySize / 2);
        }
        
        public bool IsInside(VectorTwo p)
        {
            return p.x >= 0 && p.x < xSize && p.y >= 0 && p.y < ySize;
        }
    }
}
