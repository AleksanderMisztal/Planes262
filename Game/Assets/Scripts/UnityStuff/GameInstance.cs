using System;
using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;
using Scripts.UnityStuff;

namespace Assets.Scripts.UnityStuff
{
    class GameInstance
    {
        private static GameController gc = null;

        public static void OnWelcome()
        {
            UnityEngine.Debug.Log("Connected to server!");
            UIManager.OnConnected();
        }

        public static void OnGameJoined(object opponentName, PlayerSide side, Board board)
        {
            Initialize(board);
        }

        private static void Initialize(Board board)
        {
            if (gc == null)
                gc = new GameController(board);
            throw new Exception("Game instance has already been initialized!");
        }

        public static void OnTroopsSpawned(IEnumerable<Troop> troops)
        {
            gc.BeginNextRound(troops);
            GameDisplay.BeginNextRound(troops);
        }

        public static void OnTroopMoved(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            gc.MoveTroop(position, direction, battleResults);
        }

        public static void OnGameEnded(int redScore, int blueScore)
        {
            gc.EndGame();
            gc = null;
        }

        public static void OnMessageSent(string message)
        {
            Messenger.MessageReceived(message);
        }

        public static void OnOpponentDisconnected()
        {
            // TODO: end game and go back to main screen
            throw new NotImplementedException();
        }
    }
}
