using System;
using GameDataStructures.Positioning;
using Planes262.UnityLayer.HexSystem;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField] private Camera boardCamera;
        public GridBase gridBase;

        public event Action<VectorTwo> CellClicked;

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            VectorTwo cell = gridBase.ToOffset(mousePosition);

            CellClicked?.Invoke(cell);
        }
    }
}
