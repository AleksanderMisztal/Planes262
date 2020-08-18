using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField]
        private Camera boardCamera;

        private void OnMouseDown()
        {
            var mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            var cell = MapGrid.WorldToCell(mousePosition);

            MapController.OnCellClicked(cell);
        }
    }
}
