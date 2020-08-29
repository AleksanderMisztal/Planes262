using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameManager
    {
        private readonly UIManager uiManager;
        private readonly Messenger messenger;
        private readonly Game game;

        public GameManager(Messenger messenger, UIManager uiManager, Game game)
        {
            this.messenger = messenger;
            this.uiManager = uiManager;
            this.game = game;
        }

        
        public void OnWelcome()
        {
            Debug.Log("Connected to server!");
            uiManager.ActivateMainMenu();
        }

        public void OnGameEnded(int redScore, int blueScore)
        {
            uiManager.EndGame(blueScore, redScore);
        }

        public void OnOpponentDisconnected()
        {
            uiManager.OpponentDisconnected();
        }

        
        public void OnGameJoined(string opponentName, PlayerSide side, Board board)
        {
            Debug.Log("Game joined received! Playing against " + opponentName);
            game.StartNewGame(board, side);
            messenger.ResetMessages();
            uiManager.StartTransitionIntoGame(board);
        }

        public void OnTroopSpawned(List<Troop> troops)
        {
            game.BeginNextRound(troops);
        }

        public void OnTroopMoved(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            game.MoveTroop(position, direction, battleResults);
        }

        
        public void OnMessageSent(string message)
        {
            messenger.MessageReceived(message);
        }
    }
}