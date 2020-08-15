using System.Linq;

namespace GameServer.Utils
{
    public class Hex
    {
        public static int GetDistance(VectorTwo v1, VectorTwo v2)
        {
            VectorThree cube1 = new HexOffset(v1).ToCube().ToVector();
            VectorThree cube2 = new HexOffset(v2).ToCube().ToVector();

            VectorThree dif = (cube1 - cube2).Abs();

            return new[] { dif.X, dif.Y, dif.Z }.Max();
        }

        public static VectorTwo GetAdjacentHex(VectorTwo cell, int direction)
        {
            return new HexOffset(cell).GetAdjacentHex(direction).ToVector();
        }

        public static VectorTwo[] GetNeighbours(VectorTwo cell)
        {
            return new HexOffset(cell).GetNeighbors().Select(c => c.ToVector()).ToArray();
        }

        public static VectorTwo[] GetControllZone(VectorTwo cell, int orientation)
        {
            VectorTwo[] cells = new VectorTwo[3];
            for (int i = -1; i < 2; i++)
            {
                int direction = (orientation + i + 6) % 6;
                cells[i + 1] = GetAdjacentHex(cell, direction);
            }
            return cells;
        }
    }
}
