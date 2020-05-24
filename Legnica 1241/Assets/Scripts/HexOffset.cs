using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexOffset
{
    static Vector2Int[] evenSteps = {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1)
    };
    static Vector2Int[] oddSteps = {
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1)
    };
    private int x;
    private int y;
    public HexOffset GetAdjacentHex(int direction)
    {
        Vector2Int[] steps = (y & 1) == 1 ? oddSteps : evenSteps;
        Vector2Int step = steps[direction % 6];
        return new HexOffset(x + step.x, y + step.y);
    }
    public HexOffset(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public HexOffset(Vector2Int v)
    {
        this.x = v.x;
        this.y = v.y;
    }
    public HexOffset[] GetNeighbors()
    {
        Vector2Int[] steps = (y & 1) == 1 ? oddSteps : evenSteps;
        return steps.Select(s => new HexOffset(x + s.x, y + s.y)).ToArray();
    }
    public HexCube ToCube()
    {
        var q = x - (y - (y & 1)) / 2;
        var r = y;
        var s = -q - r;
        return new HexCube(q, r, s);
    }
    public Vector2Int ToVector()
    {
        return new Vector2Int(x, y);
    }
    public override string ToString()
    {
        return "Offset(" + x + ", " + y + ")";
    }
    public override bool Equals(System.Object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
        HexOffset offset = (HexOffset)obj;
        return (x == offset.x) && (y == offset.y);
    }
}
