using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class Game
    {
        private GameState gameState;
        private readonly MapController mapController;
        private readonly Messenger messenger;
        private readonly UIManager uiManager;
        private readonly TroopController troopController;

        public Game(MapController mapController, Messenger messenger, UIManager uiManager, TroopController troopController)
        {
            this.mapController = mapController;
            this.messenger = messenger;
            this.uiManager = uiManager;
            this.troopController = troopController;
        }

        public void OnWelcome()
        {
            Debug.Log("Connected to server!");
            uiManager.OnConnected();
        }

        public void OnGameJoined(string opponentName, PlayerSide side, Board board)
        {
            gameState = new GameState(board);
            Debug.Log("Game joined received! Playing against " + opponentName);
            mapController.Initialize(side, gameState);
            messenger.ResetMessages();
            troopController.ResetForNewGame();
            uiManager.StartTransitionIntoGame(side, opponentName, board);
        }

        public void OnTroopSpawned(List<Troop> troops)
        {
            gameState.BeginNextRound(troops);
            troopController.BeginNextRound(troops);
        }

        public void OnTroopMoved(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            gameState.MoveTroop(position, direction, battleResults);
            troopController.MoveTroop(position, direction, battleResults);
        }

        public void OnGameEnded(int redScore, int blueScore)
        {
            uiManager.EndGame(blueScore, redScore);
        }

        public void OnMessageSent(string message)
        {
            messenger.MessageReceived(message);
        }

        public void OnOpponentDisconnected()
        {
            uiManager.OpponentDisconnected();
        }
    }
}