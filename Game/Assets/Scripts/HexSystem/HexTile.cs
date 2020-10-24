using System;
using UnityEngine;

namespace Planes262.HexSystem
{
    public class HexTile
    {
        public static Transform lineParent;
        public static Material lineMaterial;
        
        private static readonly Vector3 left = Vector3.left;
        private static readonly Vector3 upLeft = new Vector3(-.5f, (float)Math.Sqrt((double)3f/4));
        private static readonly Vector3[] positions = {
            -upLeft,
            -left,
            upLeft - left,
            upLeft,
            left,
            -upLeft + left,
        };
        
        private readonly LineRenderer[] lines;

        public HexTile(HexTile[] hexes, Vector3 center, float cellSize)
        {
            lines = new LineRenderer[6];
            for (int i = 0; i < 6; i++)
            {
                lines[i] = hexes[i]?.lines[(i + 3) % 6];
                if (lines[i] is null) lines[i] = GenerateLine(i, center, cellSize);
            }
        }

        public void SetColor(Color color)
        {
            foreach (LineRenderer line in lines)
            {
                line.material.color = color;
            }
        }

        private static LineRenderer GenerateLine(int i, Vector3 center, float cellSize)
        {
            LineRenderer line = new GameObject().AddComponent<LineRenderer>();
            line.name = "Line";
            line.transform.parent = lineParent;

            line.material = lineMaterial;
            line.widthMultiplier = .1f;
            line.SetPosition(0, center + positions[i] * cellSize);
            line.SetPosition(1, center + positions[(i+1)%6] * cellSize);
            
            return line;
        }
    }
}