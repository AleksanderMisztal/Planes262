﻿using GameServer.GameLogic.ServerEvents;
using GameServer.Utils;
using System.Collections.Generic;

namespace GameServer.GameLogic
{
    public class TroopAi
    {
        private readonly TroopMap troopMap;
        private readonly Board board;

        public TroopAi(TroopMap troopMap, Board board)
        {
            this.troopMap = troopMap;
            this.board = board;
        }


        public bool ShouldControll(Troop troop)
        {
            if (board.IsOutside(troop.Position)) return true;

            foreach (var cell in troop.ControllZone)
                if (board.IsInside(cell))
                    return false;

            return true;
        }

        public int GetOptimalDirection(Troop troop)
        {
            VectorTwo target = board.Center;

            int minDist = 1000000;
            int minDir = 0;
            for (int dir = -1; dir <= 1; dir += 2)
            {
                int direction = (6 + dir + troop.Orientation) % 6;
                VectorTwo neigh = Hex.GetAdjacentHex(troop.Position, direction);
                if (troopMap.Get(neigh) != null) continue;

                int dist = (target - neigh).SqrMagnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    minDir = dir;
                }
            }
            return minDir;
        }
    }
}
