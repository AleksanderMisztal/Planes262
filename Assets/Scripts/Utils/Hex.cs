using System.Linq;
using UnityEngine;

namespace Scripts.Utils
{
    public class Hex
    {
        public int Orientation { get; private set; }
        public Vector2Int Position { get; private set; }

        public Hex(Vector2Int p, int o)
        {
            Orientation = o;
            Position = p;
        }
        public void Move(int direction)
        {
            Orientation = (Orientation + direction + 6) % 6;
            Position = GetAdjacentHex(Position, Orientation);
        }

        public static int GetDistance(Vector2Int v1, Vector2Int v2)
        {
            Vector3Int cube1 = new HexOffset(v1).ToCube().ToVector();
            Vector3Int cube2 = new HexOffset(v2).ToCube().ToVector();

            Vector3Int dif = cube1 - cube2;
            int x = Mathf.Abs(dif.x), y = Mathf.Abs(dif.y), z = Mathf.Abs(dif.z);

            return new[] { x, y, z }.Max();
        }

        public static Vector2Int GetAdjacentHex(Vector2Int cell, int direction)
        {
            return new HexOffset(cell).GetAdjacentHex(direction).ToVector();
        }

        public static Vector2Int[] GetNeighbours(Vector2Int cell)
        {
            return new HexOffset(cell).GetNeighbors().Select(c => c.ToVector()).ToArray();
        }

        public static bool IsLegalMove(Vector2Int position, int orientation, Vector2Int target, out int direction)
        {
            for (direction = -1; direction <= 1; direction++)
            {
                int worldDirection = (6 + orientation + direction) % 6;
                if (GetAdjacentHex(position, worldDirection) == target) return true;
            }
            return false;
        }
    }
}
