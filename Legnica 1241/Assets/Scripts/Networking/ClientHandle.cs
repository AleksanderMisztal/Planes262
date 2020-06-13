using Scripts.GameLogic;
using Scripts.UnityStuff;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Networking
{
    public class ClientHandle
    {
        public static void Welcome(Packet packet)
        {
            string message = packet.ReadString();
            int myId = packet.ReadInt();

            Debug.Log($"Received a message: {message}");
            Client.instance.myId = myId;

            UIManager.Activate();
        }

        public static void GameJoined(Packet packet)
        {
            string oponentName = packet.ReadString();
            PlayerId side = (PlayerId)packet.ReadInt();

            GameController.Side = side;
            UIManager.SetOponentName(oponentName);
            UIManager.StartGame(side);
        }

        public static void TroopSpawned(Packet packet)
        {
            int length = packet.ReadInt();

            List<SpawnTemplate> templates = new List<SpawnTemplate>();

            for (int i = 0; i < length; i++)
            {
                SpawnTemplate template = packet.ReadSpawnTemplate();

                templates.Add(template);
            }
            NetworkingHub.OnTroopsSpawned(templates);
        }

        public static void TroopMoved(Packet packet)
        {
            Vector2Int position = packet.ReadVector2Int();
            int direction = packet.ReadInt();

            int length = packet.ReadInt();
            List<BattleResult> battleResults = new List<BattleResult>();
            for (int i = 0; i < length; i++)
            {
                battleResults.Add(packet.ReadBattleResult());
            }

            NetworkingHub.OnTroopMoved(position, direction, battleResults);
        }

        public static void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            UIManager.EndGame(blueScore, redScore);
        }
    }
}
