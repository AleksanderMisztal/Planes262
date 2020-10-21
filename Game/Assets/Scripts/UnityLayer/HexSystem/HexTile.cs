using System;
using UnityEngine;

namespace Planes262.UnityLayer.HexSystem
{
    public class HexTile
    {
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
            LineRenderer lineRenderer = new GameObject().AddComponent<LineRenderer>();

            lineRenderer.material.color = Color.white;
            lineRenderer.widthMultiplier = .1f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, center + positions[i] * cellSize);
            lineRenderer.SetPosition(1, center + positions[(i+1)%6] * cellSize);
            lineRenderer.gameObject.layer = 8;
            lineRenderer.material.color = Color.white;
            
            return lineRenderer;
        }
    }
}