using System;

namespace Planes262.GameLogic.Utils
{
    public class OrientedCell
    {
        public VectorTwo Position { get; }
        private readonly int orientation;

        public OrientedCell(VectorTwo position, int orientation)
        {
            Position = position;
            this.orientation = orientation;
        }

        public OrientedCell[] GetControlZone()
        {
            OrientedCell[] cs = new OrientedCell[3];
            for (int i = -1; i < 2; i++)
            {
                cs[i + 1] = GetAdjacent(i);
            }
            return cs;
        }

        private OrientedCell GetAdjacent(int direction)
        {
            VectorTwo position = Hex.GetAdjacentHex(Position, orientation + direction);
            return new OrientedCell(position, orientation + direction);
        }

        public int GetDirection(OrientedCell coords)
        {
            for (int i = -1; i < 2; i++)
            {
                OrientedCell c = GetAdjacent(i);
                if (c == coords) return i;
            }
            throw new Exception("Can't get there!");
        }

        public override int GetHashCode()
        {
            return orientation + 7 * (Position.X + 101 * Position.Y);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
            OrientedCell c = (OrientedCell)obj;
            return Position.X == c.Position.X && Position.Y == c.Position.Y && orientation == c.orientation;
        }
        public static bool operator ==(OrientedCell a, OrientedCell b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }
        public static bool operator !=(OrientedCell a, OrientedCell b)
        {
            return !(a == b);
        }
    }
}
