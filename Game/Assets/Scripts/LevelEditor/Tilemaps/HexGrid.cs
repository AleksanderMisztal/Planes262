using GameDataStructures.Positioning;
using UnityEngine;

namespace Planes262.LevelEditor.Tilemaps
{
    public class HexGrid
    {
        private readonly ResizableGridBase gridBase;
        private readonly GameObject[,] tiles;

        public HexGrid(ResizableGridBase gridBase)
        {
            this.gridBase = gridBase;
            tiles = new GameObject[ResizableGridBase.maxSize, ResizableGridBase.maxSize];
        }

        public void SetTile(Vector3 wp, GameObject tile)
        {
            VectorTwo coords = gridBase.ToOffset(wp);
            SetTile(coords.x, coords.y, tile);
        }

        public void SetTile(int x, int y, GameObject tile)
        {
            if (x < 0 || x >= gridBase.XSize || y < 0 || y >= gridBase.YSize) return;
            GameObject go = tiles[x, y];
            if (!(go is null)) Object.Destroy(go); 
            tiles[x, y] = tile;
        }

        public GameObject GetTile(int x, int y)
        {
            if (x < 0 || x >= ResizableGridBase.maxSize || y < 0 || y >= ResizableGridBase.maxSize) return default;
            return tiles[x, y];
        }
    }
}