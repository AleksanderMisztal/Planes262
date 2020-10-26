using System;
using UnityEngine;

namespace Planes262.LevelEditor.Tilemaps
{
    public static class LineDrawer
    {
        public static Transform lineParent;
        public static GameObject DrawHex(Vector3 center, float size)
        {
            LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
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
            lineRenderer.transform.parent = lineParent;
            return lineRenderer.gameObject;
        }
    }
}