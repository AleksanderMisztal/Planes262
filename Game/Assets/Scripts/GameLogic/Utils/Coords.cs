using Assets.Scripts.GameLogic;
using GameServer.Utils;
using System;

namespace GameServer.GameLogic
{
    public class Coords
    {
        public VectorTwo Position { get; }
        public int Orientation { get; }

        public Coords(VectorTwo position, int orientation)
        {
            Position = position;
            Orientation = orientation;
        }

        public Coords[] GetControllZone()
        {
            var cs = new Coords[3];
            for (int i = -1; i < 2; i++)
            {
                cs[i + 1] = GetAdjacent(i);
            }
            return cs;
        }

        private Coords GetAdjacent(int direction)
        {
            VectorTwo position = Hex.GetAdjacentHex(Position, Orientation + direction);
            return new Coords(position, Orientation + direction);
        }

        public int GetDirection(Coords coords)
        {
            for (int i = -1; i <= 2; i++)
            {
                var c = GetAdjacent(i);
                if (c == coords) return i;
            }
            throw new PathFindingException("Can't get there!");
        }

        public override int GetHashCode()
        {
            return Orientation + 7 * (Position.X + 101 * Position.Y);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
            Coords c = (Coords)obj;
            return Position.X == c.Position.X && Position.Y == c.Position.Y && Orientation == c.Orientation;
        }
    }
}
