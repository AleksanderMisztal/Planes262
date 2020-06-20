using Scripts.GameLogic;
using UnityEngine;

namespace Scripts.UnityStuff
{
    public class InputParser : MonoBehaviour
    {
        private GridLayout gridLayout;
        private GameController gameController;


        private void Awake()
        {
            gridLayout = FindObjectOfType<GridLayout>();
            gameController = FindObjectOfType<GameController>();
        }

        public Vector2Int GetCell(Vector3 v)
        {
            return (Vector2Int)gridLayout.WorldToCell(v);
        }

        private void OnMouseDown()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int cell = GetCell(mousePosition);

            gameController.OnCellClicked(cell);
        }
    }
}
