using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer.Utils
{
    public class MapGrid : MonoBehaviour
    {
        private static MapGrid instance;

        private GridLayout gridLayout;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }

        private void Start()
        {
            gridLayout = GetComponent<GridLayout>();
        }

        public static Vector3 CellToWorld(VectorTwo v)
        {
            Vector3Int cell = new Vector3Int(v.X, v.Y, 0);
            return instance.gridLayout.CellToWorld(cell);
        }

        public static VectorTwo WorldToCell(Vector3 v)
        {
            Vector3Int cell = instance.gridLayout.WorldToCell(v);
            return new VectorTwo(cell.x, cell.y);
        }
    }
}
