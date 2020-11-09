using System;
using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using UnityEngine;

namespace Planes262.LevelEditor.Tilemaps
{
    public class ResizableGridBase
    {
        private const int initialSize = 10;
        public const int maxSize = 100;
        public int XSize { get; private set; } = initialSize;
        public int YSize { get; private set; } = initialSize;
        private readonly float cellSize;

        public event Action GridResized;
        
        public List<VectorTwo> newReachable;
        public List<VectorTwo> newUnreachable;
        private readonly GameObject[,] tiles;
        private readonly Camera camera;

        public ResizableGridBase(float cellSize)
        {
            this.cellSize = cellSize;
            tiles = new GameObject[maxSize, maxSize];
            camera = Camera.main;

            DrawTiles();
        }

        public Vector3 GetHexCenterWp()
        {
            Vector3 wp = camera.ScreenToWorldPoint(Input.mousePosition);
            wp.z = 0;
            return Cube.ToCube(wp, cellSize).ToWorld(cellSize);
        }

        public void ResizeByDelta(int dx, int dy) => Resize(XSize + dx, YSize + dy);
        public void Resize(Board board) => Resize(board.xSize, board.ySize);

        private void Resize(int newX, int newY)
        {
            newReachable = new List<VectorTwo>();
            newUnreachable = new List<VectorTwo>();
            for (int x = 0; x < newX; x++)
            for (int y = 0; y < newY; y++)
            {
                if (IsInside(x, y))
                {
                    y = YSize - 1;
                    continue;
                }
                newReachable.Add(new VectorTwo(x, y));
                tiles[x, y].SetActive(true);
            }

            int prevX = XSize;
            int prevY = YSize;
            XSize = newX;
            YSize = newY;
            for (int x = 0; x < prevX; x++)
            for (int y = 0; y < prevY; y++)
            {
                if (IsInside(x, y))
                {
                    y = YSize - 1;
                    continue;
                }
                newUnreachable.Add(new VectorTwo(x, y));
                tiles[x, y].SetActive(false);
            }
            
            GridResized?.Invoke();
        }
        
        private void DrawTiles()
        {
            LineDrawer.lineParent = new GameObject("Line Parent").transform;
            for (int x = 0; x < maxSize; x++)
            for (int y = 0; y < maxSize; y++)
            {
                Vector3 center = Cube.FromOffset(x, y).ToWorld(cellSize);
                GameObject tile = LineDrawer.DrawHex(center, cellSize);
                tiles[x, y] = tile;
                if (!IsInside(x, y)) tile.SetActive(false);
            }
        }

        public bool IsInside(int x, int y) => x >= 0 && x < XSize && y >= 0 && y < YSize;
        public Vector3 ToWorld(int x, int y) => Cube.FromOffset(x, y).ToWorld(cellSize);
        public VectorTwo ToOffset(Vector3 wp) => Cube.ToCube(wp, cellSize).ToOffset();
    }
}