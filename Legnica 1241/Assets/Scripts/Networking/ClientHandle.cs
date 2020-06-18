using Scripts.GameLogic;
using Scripts.UnityStuff;
using Scripts.Utils;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Scripts.Networking
{
    public static class ClientHandle
    {
        private delegate void PacketHandler(Packet _packet);
        private static readonly Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>
        {
            {(int) ServerPackets.Welcome, Welcome },
            {(int) ServerPackets.GameJoined, GameJoined },
            {(int) ServerPackets.TroopSpawned, TroopSpawned },
            {(int) ServerPackets.TroopMoved, TroopMoved },
            {(int) ServerPackets.GameEnded, GameEnded },
        };

        public static void HandlePacket(string byteArray)
        {
            byte[] bytes = Serializer.Deserialize(byteArray);
            using (Packet packet = new Packet(bytes))
            {
                int packetType = packet.ReadInt();
                packetHandlers[packetType](packet);
            }
        }


        public static void Welcome(Packet packet)
        {
            string message = packet.ReadString();
            int myId = packet.ReadInt();

            Debug.Log($"Received a message: {message}");

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
