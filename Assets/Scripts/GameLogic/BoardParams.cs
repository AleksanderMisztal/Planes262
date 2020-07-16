using UnityEngine;

namespace Scripts.GameLogic
{
    public class BoardParams
    {
        public readonly int xMin;
        public readonly int xMax;
        public readonly int yMax;
        public readonly int yMin;

        public BoardParams(int xMin, int xMax, int yMin, int yMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }

        public bool IsInside(Vector2Int v)
        {
            return v.x >= xMin && v.x <= xMax && v.y >= yMin && v.y <= yMax;
        }
    }
}
