using Planes262.GameLogic.Utils;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField] private Camera boardCamera;

        private MapController mapController;
        private MapGrid mapGrid;

        public void Inject(MapController mapController, MapGrid mapGrid)
        {
            this.mapController = mapController;
            this.mapGrid = mapGrid;
        }

        private void OnMouseDown()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            VectorTwo cell = mapGrid.WorldToCell(mousePosition);

            mapController.OnCellClicked(cell);
        }
    }
}
