using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class Game
    {
        private readonly MapController mapController;
        private TroopManager troopManager;
        private readonly UnityTroopController unityTroopController;
        
        public Game(UnityTroopController unityTroopController, MapController mapController)
        {
            this.unityTroopController = unityTroopController;
            this.mapController = mapController;
        }

        
        public void StartNewGame(Board board, PlayerSide side)
        {
            troopManager = new TroopManager(board);
            mapController.StartNewGame(side, troopManager);
            unityTroopController.ResetForNewGame();
        }

        public void BeginNextRound(List<Troop> troops)
        {
            troopManager.BeginNextRound(troops);
            unityTroopController.BeginNextRound(troops);
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            troopManager.MoveTroop(position, direction, battleResults);
            unityTroopController.MoveTroop(position, direction, battleResults);
        }
    }
}