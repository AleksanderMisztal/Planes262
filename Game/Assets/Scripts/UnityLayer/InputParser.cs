using System;
using GameDataStructures.Positioning;
using Planes262.HexSystem;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField] private Camera boardCamera;
        public GridBase gridBase;

        public event Action<VectorTwo> CellClicked;
        public event Action<VectorTwo> CellInspected;

        private void Update()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            VectorTwo cell = gridBase.ToOffset(mousePosition);
            if (!gridBase.IsInside(cell.x, cell.y)) return;
            
            if (Input.GetMouseButtonDown(0))
                CellClicked?.Invoke(cell);

            if (Input.GetMouseButtonDown(1))
                CellInspected?.Invoke(cell);
        }
    }
}
