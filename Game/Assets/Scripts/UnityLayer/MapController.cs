using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using Planes262.Networking;

namespace Planes262.UnityLayer
{
    public class MapController
    {
        private PlayerSide Side { get; set; }

        private GameState gameState;
        
        private VectorTwo selectedPosition;
        private TroopDto troopDto;
        private HashSet<VectorTwo> reachableCells;
        private VectorTwo targetPosition;
        private List<int> directions;
        private ClientSend sender;
        private TileManager tileManager;


        public void Inject(ClientSend sender, TileManager tileManager)
        {
            this.sender = sender;
            this.tileManager = tileManager;
        }
        
        public void Initialize(PlayerSide side, GameState _gameState)
        {
            DeactivateTroops();
            Side = side;
            gameState = _gameState;
        }

        public void OnCellClicked(VectorTwo cell)
        {
            if (cell == selectedPosition) return;
            bool troopIsSelectedAndCanReachCell = selectedPosition != null && reachableCells.Contains(cell);
            if (troopIsSelectedAndCanReachCell)
                ChangePathOrSend(cell);
            else SelectTroop(cell);
        }

        private void ChangePathOrSend(VectorTwo cell)
        {
            if (targetPosition == cell)
                SendMoves(selectedPosition, troopDto.orientation, directions);
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
            directions = gameState.GetDirections(selectedPosition, targetPosition);
            HighlightPath(selectedPosition, troopDto.orientation, directions);
        }

        private void SelectTroop(VectorTwo cell)
        {
            DeactivateTroops();
            troopDto = gameState.GetTroopDto(cell);
            if (troopDto != null && troopDto.side == Side)
                ActivateTroopAt(cell);
        }

        private void ActivateTroopAt(VectorTwo cell)
        {
            selectedPosition = cell;
            reachableCells = gameState.GetReachableCells(cell);
            tileManager.ActivateTiles(reachableCells);
        }

        private void DeactivateTroops()
        {
            tileManager.DeactivateTiles();
            selectedPosition = null;
            troopDto = null;
            reachableCells = null;
            targetPosition = null;
            directions = null;
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
