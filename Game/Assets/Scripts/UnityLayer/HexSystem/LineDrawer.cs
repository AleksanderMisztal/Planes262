using System;
using UnityEngine;

namespace Planes262.UnityLayer.HexSystem
{
    public static class LineDrawer
    {
        public static LineRenderer DrawHex(Vector3 center, float size)
        {
            LineRenderer lineRenderer = new GameObject().AddComponent<LineRenderer>();
            Vector3 left = Vector3.left * size;
            Vector3 upLeft = new Vector3(-.5f, (float)Math.Sqrt((double)3f/4)) * size;
            Vector3[] positions = {
                center + left,
                center + upLeft,
                center + upLeft - left,
                center + -left,
                center + -upLeft,
                center + -upLeft + left,
                center + left,
            };

            lineRenderer.widthMultiplier = .1f;
            lineRenderer.positionCount = 7;
            lineRenderer.SetPositions(positions);
            return lineRenderer;
        }

        public static void SetColor(LineRenderer lineRenderer, Color color)
        {
            if (lineRenderer == null)
            {
                Debug.Log("Attempting to set color of null hex.");
                return;
            }
            lineRenderer.material.color = color;
        }
    }
}