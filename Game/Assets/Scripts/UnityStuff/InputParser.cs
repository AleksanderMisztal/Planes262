using Assets.Scripts.UnityStuff;
using Planes262.Utils;
using UnityEngine;

namespace Scripts.UnityStuff
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField]
        private Camera boardCamera;

        private void OnMouseDown()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            VectorTwo cell = MapGrid.WorldToCell(mousePosition);

            MapController.OnCellClicked(cell);
        }
    }
}
