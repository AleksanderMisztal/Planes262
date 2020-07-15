using Scripts.GameLogic;
using UnityEngine;

namespace Scripts.UnityStuff
{
    public class InputParser : MonoBehaviour
    {
        private GridLayout gridLayout;

        private Camera boardCamera;

        private void Awake()
        {
            gridLayout = FindObjectOfType<GridLayout>();
        }

        private void Start()
        {
            boardCamera = GameObject.FindGameObjectWithTag("Board Camera").GetComponent<Camera>();
        }

        public Vector2Int GetCell(Vector3 v)
        {
            return (Vector2Int)gridLayout.WorldToCell(v);
        }

        private void OnMouseDown()
        {
            Vector3 mousePosition = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int cell = GetCell(mousePosition);

            GameController.instance.OnCellClicked(cell);
        }
    }
}
