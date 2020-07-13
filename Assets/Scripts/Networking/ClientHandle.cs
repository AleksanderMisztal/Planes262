using Cysharp.Threading.Tasks;
using Scripts.GameLogic;
using Scripts.UnityStuff;
using Scripts.Utils;
using System;
using System.Collections.Generic;
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
            {(int) ServerPackets.OpponentDisconnected, OpponentDisconnected },
        };

        private static readonly Queue<Action> q = new Queue<Action>();

        public static async UniTask StartHandling()
        {
            while (q.Count > 0)
            {
                await UniTask.WaitUntil(() => GameController.AcceptsCalls);
                if (q.Count > 0) q.Dequeue()();
            }
        }

        public static void HandlePacket(string byteArray)
        {
            byte[] bytes = Serializer.Deserialize(byteArray);
            Packet packet = new Packet(bytes);
            int packetType = packet.ReadInt();
            q.Enqueue(() => packetHandlers[packetType](packet));
        }


        public static void Welcome(Packet packet)
        {
            string message = packet.ReadString();
            int myId = packet.ReadInt();

            Debug.Log($"Received a message: {message}");

            UIManager.OnConnected();
            packet.Dispose();
        }

        public static async void GameJoined(Packet packet)
        {
            string oponentName = packet.ReadString();
            PlayerId side = (PlayerId)packet.ReadInt();

            GameController.Side = side;
            await UIManager.StartGame(side, oponentName);
            packet.Dispose();
        }

        public static async void TroopSpawned(Packet packet)
        {
            packet = new Packet(packet);

            await UniTask.WaitUntil(() => UIManager.GameStarted);

            int length = packet.ReadInt();

            List<SpawnTemplate> templates = new List<SpawnTemplate>();

            for (int i = 0; i < length; i++)
            {
                SpawnTemplate template = packet.ReadSpawnTemplate();

                templates.Add(template);
            }
            NetworkingHub.OnTroopsSpawned(templates);
            packet.Dispose();
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
            packet.Dispose();
        }

        public static void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            UIManager.EndGame(blueScore, redScore);
            packet.Dispose();
        }

        public static void OpponentDisconnected(Packet packet)
        {
            UIManager.OpponentDisconnected();
            packet.Dispose();
        }
    }
}
