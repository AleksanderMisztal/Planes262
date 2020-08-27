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

        public Game(MapController mapController, Messenger messenger)
        {
            this.mapController = mapController;
            this.messenger = messenger;
        }

        public void OnWelcome()
        {
            Debug.Log("Connected to server!");
            UIManager.Instance.OnConnected();
        }

        public void OnGameJoined(string opponentName, PlayerSide side, Board board)
        {
            gameState = new GameState(board);
            Debug.Log("Game joined received! Playing against " + opponentName);
            mapController.Initialize(side, gameState);
            messenger.ResetMessages();
            TroopController.ResetForNewGame();
            UIManager.Instance.StartTransitionIntoGame(side, opponentName, board);
        }

        public void OnTroopSpawned(List<Troop> troops)
        {
            gameState.BeginNextRound(troops);
            TroopController.BeginNextRound(troops);
        }

        public void OnTroopMoved(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            gameState.MoveTroop(position, direction, battleResults);
            TroopController.MoveTroop(position, direction, battleResults);
        }

        public void OnGameEnded(int redScore, int blueScore)
        {
            UIManager.Instance.EndGame(blueScore, redScore);
        }

        public void OnMessageSent(string message)
        {
            messenger.MessageReceived(message);
        }

        public void OnOpponentDisconnected()
        {
            UIManager.Instance.OpponentDisconnected();
        }
    }
}