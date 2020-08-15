using GameServer.GameLogic;
using GameServer.Utils;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.UnityStuff
{
    public static class GCWrapper
    {
        private static GameController gc = null;

        public static IEnumerable<VectorTwo> GetReachableCells(VectorTwo position)
        {
            return gc.GetReachableCells(position);
        }

        public static List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            return gc.GetDirections(start, end);
        }


        public static void InitializeGameController(Board board)
        {
            if (gc == null)
                gc = new GameController(board);
            else
                throw new Exception("Game instance has already been initialized!");
        }

        public static void BeginNextRound(IEnumerable<Troop> troops)
        {
            gc.BeginNextRound(troops);
        }

        public static void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            gc.MoveTroop(position, direction, battleResults);
        }

        public static void GameEnded()
        {
            gc = null;
        }
    }
}
