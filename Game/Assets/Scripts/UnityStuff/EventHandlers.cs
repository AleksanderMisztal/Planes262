using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;
using Scripts.UnityStuff;
using UnityEngine;

namespace Assets.Scripts.UnityStuff
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
            GCWrapper.InitializeGame(board, side);
            UIManager.StartTransitionIntoGame(side, opponentName, board);
        }

        public static void OnTroopsSpawned(IEnumerable<Troop> troops)
        {
            GCWrapper.BeginNextRound(troops);
            TroopInstantiator.BeginNextRound(troops);
        }

        public static void OnTroopMoved(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            GCWrapper.MoveTroop(position, direction, battleResults);
        }

        public static void OnGameEnded(int redScore, int blueScore)
        {
            GCWrapper.GameEnded();
            // TODO: UIManager end game blah blah
        }

        public static void OnMessageSent(string message)
        {
            Messenger.MessageReceived(message);
        }

        public static void OnOpponentDisconnected()
        {
            // TODO: end game and go back to main screen
            GCWrapper.GameEnded();
            UIManager.OpponentDisconnected();
        }
    }
}
