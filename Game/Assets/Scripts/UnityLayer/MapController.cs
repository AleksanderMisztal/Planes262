using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Area;
using Planes262.GameLogic.Troops;
using Planes262.GameLogic.Utils;
using Planes262.Networking;

namespace Planes262.UnityLayer
{
    public class MapController
    {
        public MapController(TileManager tileManager, TroopMap troopMap)
        {
            this.tileManager = tileManager;
            this.troopMap = troopMap;
        }

        public void Inject(ClientSend sender)
        {
            this.sender = sender;
        }
        
        private readonly TileManager tileManager;
        private readonly TroopMap troopMap;
        private ClientSend sender;
        private PathFinder pathFinder;
        private PlayerSide side;
        
        private VectorTwo selectedPosition;
        private Troop selectedTroop;
        private HashSet<VectorTwo> reachableCells;
        private VectorTwo targetPosition;
        private List<int> directions;
        

        public void ResetForNewGame(PlayerSide side, Board board)
        {
            DeactivateTroops();
            this.side = side;
            pathFinder = new PathFinder(troopMap, board);
        }

        public void OnCellClicked(VectorTwo cell)
        {
            if (cell == selectedPosition) return;
            if (selectedPosition != null && reachableCells.Contains(cell))
                ChangePathOrSend(cell);
            else SelectTroop(cell);
        }

        private void ChangePathOrSend(VectorTwo cell)
        {
            if (targetPosition == cell)
                SendMoves(selectedPosition, selectedTroop.Orientation, directions);
            else SetAsTarget(cell);
        }

        private void SendMoves(VectorTwo position, int orientation, List<int> directions)
        {
            DeactivateTroops();
            foreach (int dir in directions)
            {
                sender.MoveTroop(position, dir);
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
            }
        }

        private void SetAsTarget(VectorTwo cell)
        {
            targetPosition = cell;
            directions = pathFinder.GetDirections(selectedPosition, targetPosition);
            HighlightPath(selectedPosition, selectedTroop.Orientation, directions);
        }

        private void SelectTroop(VectorTwo cell)
        {
            DeactivateTroops();
            selectedTroop = troopMap.Get(cell);
            if (selectedTroop != null && selectedTroop.Player == side)
                ActivateTroopAt(cell);
        }

        private void ActivateTroopAt(VectorTwo cell)
        {
            selectedPosition = cell;
            reachableCells = pathFinder.GetReachableCells(cell);
            tileManager.ActivateTiles(reachableCells);
        }

        private void DeactivateTroops()
        {
            selectedPosition = null;
            selectedTroop = null;
            reachableCells = null;
            targetPosition = null;
            directions = null;
            tileManager.DeactivateTiles();
    }

        private void HighlightPath(VectorTwo position, int orientation, List<int> directions)
        {
            List<VectorTwo> cells = new List<VectorTwo>();
            foreach (int dir in directions)
            {
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
                cells.Add(position);
            }
            tileManager.HighlightPath(cells);
        }
    }
}
