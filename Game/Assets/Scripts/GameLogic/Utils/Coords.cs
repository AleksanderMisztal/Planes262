using Assets.Scripts.GameLogic;
using GameServer.Utils;
using System;

namespace GameServer.GameLogic
{
    public class Coord
    {
        public VectorTwo Position { get; }
        public int Orientation { get; }

        public Coord(VectorTwo position, int orientation)
        {
            Position = position;
            Orientation = orientation;
        }

        public Coord[] GetControllZone()
        {
            var cs = new Coord[3];
            for (int i = -1; i < 2; i++)
            {
                cs[i + 1] = GetAdjacent(i);
            }
            return cs;
        }

        private Coord GetAdjacent(int direction)
        {
            VectorTwo position = Hex.GetAdjacentHex(Position, Orientation + direction);
            return new Coord(position, Orientation + direction);
        }

        public int GetDirection(Coord coords)
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
            Coord c = (Coord)obj;
            return Position.X == c.Position.X && Position.Y == c.Position.Y && Orientation == c.Orientation;
        }
    }
}
