using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class Game
    {
        private readonly UnityTroopManager unityTroopManager;
        private readonly MapController mapController;
        
        public Game(UnityTroopManager unityTroopManager, MapController mapController)
        {
            this.unityTroopManager = unityTroopManager;
            this.mapController = mapController;
        }

        
        public void StartNewGame(Board board, PlayerSide side)
        {
            unityTroopManager.ResetForNewGame(board);
            mapController.StartNewGame(side, unityTroopManager);
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            unityTroopManager.BeginNextRound(troops);
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            unityTroopManager.MoveTroop(position, direction, battleResults);
        }
    }
}