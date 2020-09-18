using System;
using System.Collections.Generic;
using GameDataStructures;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using ITroop = Planes262.GameLogic.Troops.ITroop;

namespace Planes262.UnityLayer
{
    public class MapController
    {
        public MapController(TileManager tileManager, TroopMap troopMap, EventHandler<MoveAttemptEventArgs> troopMoveHandler)
        {
            this.tileManager = tileManager;
            this.troopMap = troopMap;
            this.troopMoveHandler = troopMoveHandler;
        }
        
        private readonly TileManager tileManager;
        private readonly TroopMap troopMap;
        private readonly EventHandler<MoveAttemptEventArgs> troopMoveHandler;
        
        private PathFinder pathFinder;
        private PlayerSide side;
        private PlayerSide activePlayer = PlayerSide.Red;
        public bool IsLocal = false;
        
        private VectorTwo selectedPosition;
        private ITroop selectedTroop;
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
                troopMoveHandler?.Invoke(this, new MoveAttemptEventArgs(activePlayer, position, dir));
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
            if (selectedTroop != null && (selectedTroop.Player == side || IsLocal) && selectedTroop.Player == activePlayer)
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

        public void ToggleActivePlayer()
        {
            activePlayer = activePlayer.Opponent();
        }
    }
}
