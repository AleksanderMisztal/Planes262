using System;
using GameDataStructures;
using GameDataStructures.Positioning;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField] private Camera boardCamera;
        [SerializeField] private MapGrid mapGrid;

        public event Action<VectorTwo> CellClicked;

        private void OnMouseDown()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            VectorTwo cell = mapGrid.WorldToCell(mousePosition);

            CellClicked?.Invoke(cell);
        }
    }
}
