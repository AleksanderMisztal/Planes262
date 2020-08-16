using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;
using Scripts.Networking;

namespace Assets.Scripts.UnityStuff
{
    public static class GameController
    {
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
                    directions = GameState.GetDirections(troopPosition, targetPosition);
                    HighlightPath(troopPosition, troopDto.orientation, directions);
                }
            }
            else
            {
                troopDto = GameState.GetTroopSide(cell);
                if (troopDto != null && troopDto.side == Side)
                {
                    troopPosition = cell;
                    reachableCells = GameState.GetReachableCells(cell);
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
    }
}
