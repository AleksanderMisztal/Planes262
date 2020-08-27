using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class Game
    {
        private readonly TroopManager troopManager;
        private readonly MapController mapController;
        
        public Game(TroopManager troopManager, MapController mapController)
        {
            this.troopManager = troopManager;
            this.mapController = mapController;
        }

        
        public void StartNewGame(Board board, PlayerSide side)
        {
            troopManager.ResetForNewGame(board);
            mapController.StartNewGame(side, troopManager);
        }

        public void BeginNextRound(List<Troop> troops)
        {
            troopManager.BeginNextRound(troops);
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            troopManager.MoveTroop(position, direction, battleResults);
        }
    }
}