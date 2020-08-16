using System;
using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;
using Scripts.Networking;

namespace Assets.Scripts.UnityStuff
{
    public static class GCWrapper
    {
        private static GameController gc = null;
        public static PlayerSide Side { get; private set; }

        private static VectorTwo troopPosition = null;
        private static TroopDto troopDto = null;
        private static HashSet<VectorTwo> reachableCells = null;
        private static VectorTwo targetPosition = null;
        private static List<int> directions = null;


        public static void OnCellClicked(VectorTwo cell)
        {
            if (troopPosition != null && reachableCells.Contains(cell))
            {
                if (targetPosition != null && targetPosition == cell)
                {
                    SendMoves(troopPosition, troopDto.orientation, directions);
                }
                else
                {
                    targetPosition = cell;
                    directions = gc.GetDirections(troopPosition, targetPosition);
                    HighlightPath(troopPosition, troopDto.orientation, directions);
                }
            }
            else
            {
                troopDto = gc.GetTroopSide(cell);
                if (troopDto != null && troopDto.side == Side)
                {
                    troopPosition = cell;
                    reachableCells = gc.GetReachableCells(cell);
                }
            }
        }

        private static void HighlightPath(VectorTwo position, int orientation, List<int> directions)
        {
            var cells = new List<VectorTwo>();
            foreach (var dir in directions)
            {
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
                cells.Add(position);
            }
            TileManager.HighlightPath(cells);
        }

        private static void SendMoves(VectorTwo position, int orientation, List<int> directions)
        {
            foreach (var dir in directions)
            {
                ClientSend.MoveTroop(position, dir);
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
            }
        }


        public static void InitializeGame(Board board, PlayerSide side)
        {
            Side = side;
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
