using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using Planes262.Networking;

namespace Planes262.UnityLayer
{
    public static class MapController
    {
        private static PlayerSide Side { get; set; }

        private static VectorTwo selectedPosition;
        private static TroopDto troopDto;
        private static HashSet<VectorTwo> reachableCells;
        private static VectorTwo targetPosition;
        private static List<int> directions;
        private static ClientSend sender;

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
            foreach (int dir in directions)
            {
                sender.MoveTroop(position, dir);
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
            List<VectorTwo> cells = new List<VectorTwo>();
            foreach (int dir in directions)
            {
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
                cells.Add(position);
            }
            TileManager.HighlightPath(cells);
        }

        public static void SetSender(ClientSend _sender)
        {
            sender = _sender;
        }
    }
}
