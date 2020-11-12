using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures.Positioning;

namespace Planes262.GameLogic
{
    public class OrientedCell
    {
        public VectorTwo Position { get; }
        private readonly int orientation;

        public OrientedCell(VectorTwo position, int orientation)
        {
            Position = position;
            this.orientation = (orientation%6+6)%6;
        }

        public IEnumerable<OrientedCell> GetReachable()
        {
            return new[] {-1, 0, 1}.Select(GetAdjacent);
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
            return orientation + 7 * (Position.x + 101 * Position.y);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType()) return false;
            OrientedCell c = (OrientedCell)obj;
            return Position.x == c.Position.x && Position.y == c.Position.y && orientation == c.orientation;
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

        public override string ToString()
        {
            return "" + Position + orientation;
        }
    }
}
