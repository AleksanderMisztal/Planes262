using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using Planes262.Networking;

namespace Planes262.UnityLayer
{
    public class MapController
    {
        public MapController(TileManager tileManager)
        {
            this.tileManager = tileManager;
        }

        public void Inject(ClientSend sender)
        {
            this.sender = sender;
        }

        private PlayerSide Side { get; set; }

        private TroopController troopController;
        
        private VectorTwo selectedPosition;
        private TroopDto troopDto;
        private HashSet<VectorTwo> reachableCells;
        private VectorTwo targetPosition;
        private List<int> directions;
        
        private ClientSend sender;
        private readonly TileManager tileManager;
        
        public void StartNewGame(PlayerSide side, TroopController troopController)
        {
            DeactivateTroops();
            Side = side;
            this.troopController = troopController;
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
            directions = troopController.GetDirections(selectedPosition, targetPosition);
            HighlightPath(selectedPosition, troopDto.orientation, directions);
        }

        private void SelectTroop(VectorTwo cell)
        {
            DeactivateTroops();
            troopDto = troopController.GetTroopDto(cell);
            if (troopDto != null && troopDto.side == Side)
                ActivateTroopAt(cell);
        }

        private void ActivateTroopAt(VectorTwo cell)
        {
            selectedPosition = cell;
            reachableCells = troopController.GetReachableCells(cell);
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
