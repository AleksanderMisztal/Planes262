using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using UnityEngine;

namespace Planes262.UnityLayer.HexSystem
{
    public class GridBase
    {
        private readonly int xSize;
        private readonly int ySize;
        private readonly float cellSize;
        private readonly LineRenderer[,] tiles;
        
        private IEnumerable<VectorTwo> activePositions = new List<VectorTwo>();
        private IEnumerable<VectorTwo> highlightedPath = new List<VectorTwo>();

        private static readonly Color active = new Color(0, 0, 255, 255);
        private static readonly Color activeBlocked = new Color(0, 0, 0, 127);
        private static readonly Color onPath = new Color(255, 0, 0, 127);

        public GridBase(Board board, float cellSize)
        {
            xSize = board.xSize;
            ySize = board.ySize;
            this.cellSize = cellSize;
            tiles = new LineRenderer[xSize, ySize];

            CreateBoard();
        }

        private void CreateBoard()
        {
            for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
            {
                Vector3 center = Cube.FromOffset(x, y).ToWorld(cellSize);
                tiles[x, y] = LineDrawer.DrawHex(center, cellSize, Color.white);
            }
        }

        public bool IsInside(int x, int y) => !(x < 0 || x >= xSize || y < 0 || y >= ySize);
        public Vector3 ToWorld(VectorTwo pos) => ToWorld(pos.x, pos.y);
        public Vector3 ToWorld(int x, int y) => Cube.FromOffset(x, y).ToWorld(cellSize);
        public VectorTwo ToOffset(Vector3 wp) => Cube.ToCube(wp, cellSize).ToOffset();

        public void SetReachableTiles(IEnumerable<VectorTwo> reachable)
        {
            ResetAllTiles();
            foreach (VectorTwo pos in reachable)
                LineDrawer.SetColor(GetTile(pos), active);
            activePositions = reachable;
        }

        public void SetReachableTilesBlocked(HashSet<VectorTwo> reachable)
        {
            ResetAllTiles();
            foreach (VectorTwo pos in reachable)
                LineDrawer.SetColor(GetTile(pos), activeBlocked);
            activePositions = reachable;
        }

        public void HighlightPath(IEnumerable<VectorTwo> path)
        {
            foreach (VectorTwo pos in highlightedPath)
                LineDrawer.SetColor(GetTile(pos), active);
            foreach (VectorTwo pos in path)
                LineDrawer.SetColor(GetTile(pos), onPath);
            highlightedPath = path;
        }

        public void ResetAllTiles()
        {
            foreach (VectorTwo pos in activePositions)
                LineDrawer.SetColor(GetTile(pos), Color.white);
            foreach (VectorTwo pos in highlightedPath)
                LineDrawer.SetColor(GetTile(pos), Color.white);
            activePositions = new List<VectorTwo>();
            highlightedPath = new List<VectorTwo>();
        }

        private LineRenderer GetTile(VectorTwo position)
        {
            try
            {
                return tiles[position.x, position.y];
            }
            catch
            {
                return null;
            }
        }
    }
}