using Planes262.GameLogic.Utils;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField] private Camera boardCamera;

        private MapController mapController;

        public void SetMapController(MapController mapController)
        {
            this.mapController = mapController;
        }

        private void OnMouseDown()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            VectorTwo cell = MapGrid.WorldToCell(mousePosition);

            mapController.OnCellClicked(cell);
        }
    }
}
