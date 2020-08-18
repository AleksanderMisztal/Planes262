using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using Planes262.Networking;

namespace Planes262.UnityLayer
{
    public static class MapController
    {
        public static PlayerSide Side { get; private set; }

        private static VectorTwo selectedPosition = null;
        private static TroopDto troopDto = null;
        private static HashSet<VectorTwo> reachableCells = null;
        private static VectorTwo targetPosition = null;
        private static List<int> directions = null;

        public static void Initialize(PlayerSide side)
        {
            DeactivateTroops();
            Side = side;
        }

        public static void OnCellClicked(VectorTwo cell)
        {
            if (cell == selectedPosition) return;
            bool troopIsSelectedAndCanReachCell = selectedPosition != null && reachableCells.Contains(cell);
            if (troopIsSelectedAndCanReachCell)
                ChangePathOrSend(cell);
            else SelectTroop(cell);
        }

        private static void ChangePathOrSend(VectorTwo cell)
        {
            if (targetPosition == cell)
                SendMoves(selectedPosition, troopDto.orientation, directions);
            else SetAsTarget(cell);
        }

        private static void SendMoves(VectorTwo position, int orientation, List<int> directions)
        {
            DeactivateTroops();
            foreach (var dir in directions)
            {
                ClientSend.MoveTroop(position, dir);
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
            }
        }

        private static void SetAsTarget(VectorTwo cell)
        {
            targetPosition = cell;
            directions = GameState.GetDirections(selectedPosition, targetPosition);
            HighlightPath(selectedPosition, troopDto.orientation, directions);
        }

        private static void SelectTroop(VectorTwo cell)
        {
            DeactivateTroops();
            troopDto = GameState.GetTroopDto(cell);
            if (troopDto != null && troopDto.side == Side)
                ActivateTroopAt(cell);
        }

        private static void ActivateTroopAt(VectorTwo cell)
        {
            selectedPosition = cell;
            reachableCells = GameState.GetReachableCells(cell);
            TileManager.ActivateTiles(reachableCells);
        }

        private static void DeactivateTroops()
        {
            TileManager.DeactivateTiles();
            selectedPosition = null;
            troopDto = null;
            reachableCells = null;
            targetPosition = null;
            directions = null;
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
    }
}
