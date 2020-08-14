﻿using GameServer.Utils;

namespace GameServer.GameLogic
{
    public class Board
    {
        public readonly int xMax;
        public readonly int yMax;

        public Vector2Int Center { get; private set; }

        public Board(int xMax, int yMax)
        {
            this.xMax = xMax;
            this.yMax = yMax;

            Center = new Vector2Int(xMax / 2, yMax / 2);
        }

        public bool IsOutside(Vector2Int p)
        {
            return p.X < 0 || p.X > xMax || p.Y < 0 || p.Y > yMax;
        }

        public bool IsInside(Vector2Int p)
        {
            return !IsOutside(p);
        }

        public static readonly Board standard = new Board(20, 12);
        public static readonly Board test = new Board(8, 5);
    }
}
