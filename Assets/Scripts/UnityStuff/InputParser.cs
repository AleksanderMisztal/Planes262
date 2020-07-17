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

        public Vector2Int GetCell(Vector3 v)
        {
            return (Vector2Int)gridLayout.WorldToCell(v);
        }

        private void OnMouseDown()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int cell = GetCell(mousePosition);

            Debug.Log("Mouse (screen): " + Input.mousePosition);
            Debug.Log("Mouse (world): " + mousePosition);
            Debug.Log("Cell: " + cell);


            GameController.OnCellClicked(cell);
        }
    }
}
