using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCube
{
    static Vector3Int[] steps = { 
        new Vector3Int(1, -1, 0), 
        new Vector3Int(1, 0, -1), 
        new Vector3Int(0, 1, -1), 
        new Vector3Int(0, -1, 1), 
        new Vector3Int(-1, 1, 0), 
        new Vector3Int(-1, 0, 1) 
    };
    private int q, r, s;
    public HexCube(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }
    public HexCube(Vector3Int v)
    {
        this.q = v.x;
        this.r = v.y;
        this.s = v.z;
    }
    public Vector3Int ToVector()
    {
        return new Vector3Int(q, r, s);
    }
    public HexOffset ToOffset()
    {
        int y = r;
        int x = q + (y - (y & 1)) / 2;
        return new HexOffset(x, y);
    }
    public override string ToString()
    {
        return "HexCube(" + q + ", " + r + ", " + s + ")";
    }
    public override bool Equals(System.Object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
        HexCube cube = (HexCube)obj;
        return (q == cube.q) && (r == cube.r) && (s == cube.s);
    }
}
