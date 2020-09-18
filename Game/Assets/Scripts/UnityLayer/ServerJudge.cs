using System.Collections.Generic;
using GameDataStructures;
using Planes262.GameLogic.Troops;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class ServerJudge
    {
        private readonly UIManager uiManager;
        private readonly Messenger messenger;
        private readonly Game game;

        public ServerJudge(Messenger messenger, UIManager uiManager, Game game)
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
            string message = $"Final score: red: {redScore}, blue: {blueScore}";
            uiManager.EndGame(message, 1.5f);
            game.OnGameEnded();
        }

        public void OnOpponentDisconnected()
        {
            uiManager.EndGame("Opponent has disconnected :(", 0);
            game.OnGameEnded();
        }

        
        public void OnGameJoined(string opponentName, PlayerSide side, Board board)
        {
            Debug.Log("Game joined received! Playing against " + opponentName);
            game.StartNewGame(board, side);
            messenger.ResetMessages();
            uiManager.TransitionIntoGame(board);
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