using GameServer.Utils;
using UnityEngine;

namespace Assets.Scripts.UnityStuff
{
    class Grid : MonoBehaviour
    {
        private static Grid instance;

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
            var cell = new Vector3Int(v.X, v.Y, 0);
            return instance.gridLayout.CellToWorld(cell);
        }

        public static VectorTwo WorldToCell(Vector3 v)
        {
            var cell = instance.gridLayout.WorldToCell(v);
            return new GameServer.Utils.VectorTwo(cell.x, cell.y);
        }
    }
}
