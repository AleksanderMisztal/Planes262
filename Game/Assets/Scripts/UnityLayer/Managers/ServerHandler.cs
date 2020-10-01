using System.Collections.Generic;
using GameDataStructures;
using Planes262.GameLogic.Troops;
using UnityEngine;

namespace Planes262.UnityLayer.Managers
{
    public class ServerHandler
    {
        private readonly UIManager uiManager;
        private readonly Messenger messenger;
        private readonly GameManager gameManager;
        private readonly ClockDisplay clockDisplay;

        public ServerHandler(Messenger messenger, UIManager uiManager, GameManager gameManager, ClockDisplay clockDisplay)
        {
            this.messenger = messenger;
            this.uiManager = uiManager;
            this.gameManager = gameManager;
            this.clockDisplay = clockDisplay;
        }

        
        public void OnWelcome()
        {
            Debug.Log("Connected to server!");
            uiManager.ActivateMainMenu();
        }

        public void OnGameEnded(int redScore, int blueScore)
        {
            uiManager.EndGame($"Final score: red: {redScore}, blue: {blueScore}", 1.5f);
            gameManager.OnGameEnded();
        }

        public void OnOpponentDisconnected()
        {
            uiManager.EndGame("Opponent has disconnected :(", 0);
            gameManager.OnGameEnded();
        }

        public void OnLostOnTime(PlayerSide loser)
        {
            uiManager.EndGame($"Player {loser} lost on time :(", 0);
            gameManager.OnGameEnded();
        }

        
        public void OnGameJoined(string opponentName, PlayerSide side, Board board)
        {
            Debug.Log("Game joined received! Playing against " + opponentName);
            gameManager.StartNewGame(board, side);
            messenger.ResetMessages();
            uiManager.TransitionIntoGame(board);
        }

        public void OnTroopSpawned(IEnumerable<TroopDto> troops, TimeInfo timeInfo)
        {
            clockDisplay.ToggleActivePlayer(timeInfo);
            gameManager.BeginNextRound(troops);
        }

        public void OnTroopMoved(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            gameManager.MoveTroop(position, direction, battleResults);
        }

        
        public void OnMessageSent(string message)
        {
            messenger.MessageReceived(message);
        }
    }
}