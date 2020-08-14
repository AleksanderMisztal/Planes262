using UnityEngine;

namespace Scripts.GameLogic
{
    public class Board
    {
        public readonly int xMax;
        public readonly int yMax;

        public Board(int xMax, int yMax)
        {
            this.xMax = xMax;
            this.yMax = yMax;
        }

        public bool IsOutside(Vector2Int v)
        {
            return v.x < 0 || v.x > xMax || v.y < 0 || v.y > yMax;
        }

        public bool IsInside(Vector2Int v)
        {
            return !IsOutside(v);
        }
    }
}
