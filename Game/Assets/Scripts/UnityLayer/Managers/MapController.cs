using System;
using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Planes262.GameLogic;
using Planes262.UnityLayer.HexSystem;

namespace Planes262.UnityLayer.Managers
{
    public class MapController
    {
        public MapController(GridBase tileManager, TroopMap troopMap, Action<MoveAttemptEventArgs> troopMoveHandler)
        {
            this.tileManager = tileManager;
            this.troopMap = troopMap;
            this.troopMoveHandler = troopMoveHandler;
        }
        
        private readonly GridBase tileManager;
        private readonly TroopMap troopMap;
        private readonly Action<MoveAttemptEventArgs> troopMoveHandler;
        
        private PathFinder pathFinder;
        private PlayerSide playerSide;
        private PlayerSide activePlayer = PlayerSide.Red;
        public bool isLocal = false;

        private bool isPositionSelected;
        private bool isTargetSelected;
        private VectorTwo selectedPosition;
        private ITroop selectedTroop;
        private HashSet<VectorTwo> reachableCells;
        private VectorTwo targetPosition;
        private List<int> directions;
        

        public void ResetForNewGame(PlayerSide side, Board board)
        {
            DeactivateTroops();
            playerSide = side;
            activePlayer = PlayerSide.Blue;
            pathFinder = new PathFinder(troopMap, board);
        }

        public void OnCellClicked(VectorTwo cell)
        {
            if (isPositionSelected && cell == selectedPosition) return;
            if (isPositionSelected && reachableCells.Contains(cell))
                ChangePathOrSend(cell);
            else SelectTroop(cell);
        }

        private void ChangePathOrSend(VectorTwo cell)
        {
            if (isTargetSelected && targetPosition == cell)
                SendMoves(selectedPosition, selectedTroop.Orientation, directions);
            else SetAsTarget(cell);
        }

        private void SendMoves(VectorTwo position, int orientation, List<int> moveDirections)
        {
            DeactivateTroops();
            foreach (int dir in moveDirections)
            {
                troopMoveHandler(new MoveAttemptEventArgs(activePlayer, position, dir));
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
            }
        }

        private void SetAsTarget(VectorTwo cell)
        {
            isTargetSelected = true;
            targetPosition = cell;
            directions = pathFinder.GetDirections(selectedPosition, targetPosition);
            HighlightPath(selectedPosition, selectedTroop.Orientation, directions);
        }

        private void SelectTroop(VectorTwo cell)
        {
            DeactivateTroops();
            selectedTroop = troopMap.Get(cell);
            if (selectedTroop != null && (selectedTroop.Player == playerSide || isLocal) && selectedTroop.Player == activePlayer)
                ActivateTroopAt(cell);
        }

        private void ActivateTroopAt(VectorTwo cell)
        {
            isPositionSelected = true;
            selectedPosition = cell;
            reachableCells = pathFinder.GetReachableCells(cell);
            tileManager.SetReachableTiles(reachableCells);
        }

        private void DeactivateTroops()
        {
            isPositionSelected = false;
            selectedTroop = null;
            reachableCells = null;
            isTargetSelected = false;
            directions = null;
            tileManager.ResetAllTiles();
    }

        private void HighlightPath(VectorTwo position, int orientation, List<int> moveDirections)
        {
            List<VectorTwo> cells = new List<VectorTwo>();
            foreach (int dir in moveDirections)
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
