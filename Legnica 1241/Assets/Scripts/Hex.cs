using System.Linq;
using UnityEngine;

public class Hex
{
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
}
