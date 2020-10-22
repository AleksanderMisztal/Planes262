using GameDataStructures;
using GameDataStructures.Positioning;
using UnityEngine;

namespace Planes262.UnityLayer.HexSystem
{
    public class HexInspector
    {
        private readonly GridBase gridBase;
        private readonly string[,] tiles;
        private readonly int xSize;
        private readonly int ySize;

        public HexInspector(Board board, GridBase gridBase)
        {
            this.gridBase = gridBase;
            xSize = board.xSize;
            ySize = board.ySize;
            tiles = new string[xSize, ySize];

            Initialize();
        }

        private void Initialize()
        {
            for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
            {
                tiles[x, y] = Random.Range(0, 10).ToString();
            }
        }

        public void Inspect(VectorTwo cell)
        {
            Debug.Log(cell + " => " + tiles[cell.x, cell.y]);
        }
    }
}