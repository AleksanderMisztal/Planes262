﻿using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using UnityEngine;

namespace Planes262.HexSystem
{
    public class GridBase
    {
        private readonly int xSize;
        private readonly int ySize;
        private readonly float cellSize;
        private readonly Vector3 offset;
        private readonly HexTile[,] tiles;
        
        private IEnumerable<VectorTwo> activePositions = new List<VectorTwo>();
        private IEnumerable<VectorTwo> highlightedPath = new List<VectorTwo>();

        private static readonly Color active = new Color(0, 128, 0, 127);
        private static readonly Color activeBlocked = new Color(0, 0, 0, 127);
        private static readonly Color onPath = new Color(128, 0, 0, 127);

        public GridBase(Board board) : this(board, 1, Vector3.zero) { }

        private GridBase(Board board, float cellSize, Vector3 offset)
        {
            xSize = board.xSize;
            ySize = board.ySize;
            this.cellSize = cellSize;
            offset.z = 0;
            this.offset = offset;
            tiles = new HexTile[xSize, ySize];

            CreateBoard();
        }

        private void CreateBoard()
        {
            return;
            HexTile.lineParent = new GameObject("Line Parent").transform;
            for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
            {
                Vector3 center = offset + Cube.FromOffset(x, y).ToWorld(cellSize);
                HexTile[] neighs = new HexTile[6];
                for (int i = 0; i < 6; i++)
                    neighs[i] = GetTile(Hex.GetAdjacentHex(new VectorTwo(x, y), i));
                tiles[x, y] = new HexTile(neighs, center, cellSize);
            }
        }

        public bool IsInside(int x, int y) => x >= 0 && x < xSize && y >= 0 && y < ySize;
        public bool IsInside(VectorTwo pos) => IsInside(pos.x, pos.y);
        public Vector3 ToWorld(VectorTwo pos) => ToWorld(pos.x, pos.y);
        public Vector3 ToWorld(int x, int y) => offset + Cube.FromOffset(x, y).ToWorld(cellSize);
        public VectorTwo ToOffset(Vector3 wp) => Cube.ToCube(wp - offset, cellSize).ToOffset();

        public void SetReachableTiles(IEnumerable<VectorTwo> reachable)
        {
            ResetAllTiles();
            foreach (VectorTwo pos in reachable)
                GetTile(pos)?.SetColor(active);
            activePositions = reachable;
        }

        public void SetReachableTilesBlocked(HashSet<VectorTwo> reachable)
        {
            ResetAllTiles();
            foreach (VectorTwo pos in reachable)
                GetTile(pos)?.SetColor(activeBlocked);
            activePositions = reachable;
        }

        public void HighlightPath(IEnumerable<VectorTwo> path)
        {
            foreach (VectorTwo pos in highlightedPath)
                GetTile(pos)?.SetColor(active);
            foreach (VectorTwo pos in path)
                GetTile(pos)?.SetColor(onPath);
            highlightedPath = path;
        }

        public void ResetAllTiles()
        {
            foreach (VectorTwo pos in activePositions)
                GetTile(pos)?.SetColor(Color.white);
            foreach (VectorTwo pos in highlightedPath)
                GetTile(pos)?.SetColor(Color.white);
            activePositions = new List<VectorTwo>();
            highlightedPath = new List<VectorTwo>();
        }

        private HexTile GetTile(VectorTwo position)
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