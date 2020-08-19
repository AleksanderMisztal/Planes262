using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic
{
    public class Board
    {
        public readonly int XMax;
        public readonly int YMax;

        public Board(int xMax, int yMax)
        {
            XMax = xMax;
            YMax = yMax;
        }

        public bool IsOutside(VectorTwo p)
        {
            return p.X < 0 || p.X > XMax || p.Y < 0 || p.Y > YMax;
        }

        public bool IsInside(VectorTwo p)
        {
            return !IsOutside(p);
        }
    }
}
