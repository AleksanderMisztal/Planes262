using Scripts.GameLogic;
using UnityEngine;

namespace Scripts.UnityStuff
{
    public class InputParser : MonoBehaviour
    {
        private GridLayout gridLayout;

        [SerializeField]
        private Camera boardCamera;

        private void Awake()
        {
            gridLayout = FindObjectOfType<GridLayout>();
        }

        private void OnMouseDown()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int cell = (Vector2Int)gridLayout.WorldToCell(mousePosition);

            GameController.OnCellClicked(cell);
        }
    }
}
