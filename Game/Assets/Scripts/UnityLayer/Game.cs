using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class Game
    {
        private readonly MapController mapController;
        private TroopController troopController;
        private readonly UnityTroopController unityTroopController;
        
        public Game(UnityTroopController unityTroopController, MapController mapController)
        {
            this.unityTroopController = unityTroopController;
            this.mapController = mapController;
        }

        public void StartNewGame(Board board, PlayerSide side)
        {
            troopController = new TroopController(board);
            mapController.StartNewGame(side, troopController);
            unityTroopController.ResetForNewGame();
        }

        public void BeginNextRound(List<Troop> troops)
        {
            troopController.BeginNextRound(troops);
            unityTroopController.BeginNextRound(troops);
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            troopController.MoveTroop(position, direction, battleResults);
            unityTroopController.MoveTroop(position, direction, battleResults);
        }
    }
}