using System;
using GameDataStructures.Positioning;
using UnityEngine;

namespace Planes262.LevelEditor.Tilemaps
{
    public class Cube
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        public Cube(int x, int y)
        {
            this.x = x;
            this.y = y;
            z = -x - y;
        }

        public override string ToString() => x + " " + y + " " + z;

        public static Cube ToCube(Vector3 wp, float cellSize)
        {
            float q = 2f/3 * wp.x / cellSize;
            float r = (-1f/3 * wp.x  - (float) Math.Sqrt(3)/3 * wp.y) / cellSize;
            return Round(q, r);
        }

        private static Cube Round(float q, float r)
        {
            float x = q;
            float y = r;
            float z = - q - r;
            
            int rx = (int) Math.Round(x);
            int ry = (int) Math.Round(y);
            int rz = (int) Math.Round(z);

            float xDiff = Math.Abs(rx - x);
            float yDiff = Math.Abs(ry - y);
            float zDiff = Math.Abs(rz - z);

            if (xDiff > yDiff && xDiff > zDiff) rx = -ry - rz;
            else if (yDiff > zDiff) ry = -rx - rz;
            else rz = -rx - ry;
            
            
            return new Cube(rx, ry);
        }

        public Vector3 ToWorld(float cellSize)
        {
            float wpx = cellSize * (     3f/2 * x);
            float wpy = -cellSize * ((float) Math.Sqrt(3) / 2 * x + (float) Math.Sqrt(3) * y);
            
            return new Vector3(wpx, wpy);
        }

        public VectorTwo ToOffset()
        {
            int col = x;
            int row = z + (x - (x&1)) / 2;
            return new VectorTwo(col, row);
        }

        public static Cube FromOffset(int col, int row)
        {
            int x = col;
            int z = row - (col - (col & 1)) / 2;
            int y = -x - z;
            return new Cube(x, y);
        }

        public static Cube FromOffset(VectorTwo v) => FromOffset(v.x, v.y);
    }
}