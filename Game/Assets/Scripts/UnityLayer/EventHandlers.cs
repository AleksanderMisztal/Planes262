using System.Collections.Generic;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class EventHandlers
    {
        public static void OnWelcome()
        {
            Debug.Log("Connected to server!");
            UIManager.OnConnected();
        }

        public static void OnGameJoined(string opponentName, PlayerSide side, Board board)
        {
            Debug.Log("Game joined received! Playing against " + opponentName);
            TroopController.ResetForNewGame();
            MapController.Initialize(side);
            UIManager.StartTransitionIntoGame(side, opponentName, board);
        }

        public static void OnTroopsSpawned(IEnumerable<Troop> troops)
        {
            TroopController.BeginNextRound(troops);
        }

        public static void OnTroopMoved(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            TroopController.MoveTroop(position, direction, battleResults);
        }

        public static void OnGameEnded(int redScore, int blueScore)
        {
            UIManager.EndGame(blueScore, redScore);
        }

        public static void OnMessageSent(string message)
        {
            Messenger.MessageReceived(message);
        }

        public static void OnOpponentDisconnected()
        {
            UIManager.OpponentDisconnected();
        }
    }
}
