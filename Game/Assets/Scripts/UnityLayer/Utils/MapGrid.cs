using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer.Utils
{
    public class MapGrid : MonoBehaviour
    {
        private GridLayout gridLayout;

        private void Start()
        {
            gridLayout = GetComponent<GridLayout>();
        }

        public Vector3 CellToWorld(VectorTwo v)
        {
            Vector3Int cell = new Vector3Int(v.X, v.Y, 0);
            return gridLayout.CellToWorld(cell);
        }

        public VectorTwo WorldToCell(Vector3 v)
        {
            Vector3Int cell = gridLayout.WorldToCell(v);
            return new VectorTwo(cell.x, cell.y);
        }
    }
}
