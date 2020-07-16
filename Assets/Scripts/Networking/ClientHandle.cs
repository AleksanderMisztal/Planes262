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
            {(int) ServerPackets.MessageReceived, MessageReceived },
        };

        public static void HandlePacket(string byteArray)
        {
            byte[] bytes = Serializer.Deserialize(byteArray);
            Packet packet = new Packet(bytes);
            int packetType = packet.ReadInt();
            packetHandlers[packetType](packet);
        }


        public static void Welcome(Packet packet)
        {
            string message = packet.ReadString();
            int myId = packet.ReadInt();

            Debug.Log($"Received a message: {message}");

            UIManager.OnConnected();
            packet.Dispose();
        }

        public static void GameJoined(Packet packet)
        {
            string oponentName = packet.ReadString();
            PlayerId side = (PlayerId)packet.ReadInt();
            BoardParams board = packet.ReadBoardParams();

            GameController.Side = side;
            GameController.Board = board;
            packet.Dispose();
            GameController.StartGame(side, oponentName, board);
        }

        public static void TroopSpawned(Packet packet)
        {
            packet = new Packet(packet);

            int length = packet.ReadInt();

            List<SpawnTemplate> templates = new List<SpawnTemplate>();

            for (int i = 0; i < length; i++)
            {
                SpawnTemplate template = packet.ReadSpawnTemplate();

                templates.Add(template);
            }
            GameController.StartNextRound(templates);
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

            packet.Dispose();
            GameController.OnTroopMoved(position, direction, battleResults);
        }

        public static async void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();
            packet.Dispose();

            GameController.EndGame();
            await UIManager.EndGame(blueScore, redScore);
        }

        public static void OpponentDisconnected(Packet packet)
        {
            packet.Dispose();
            GameController.EndGame();
            UIManager.OpponentDisconnected();
        }

        public static void MessageReceived(Packet packet)
        {
            string message = packet.ReadString();
            Messenger.MessageReceived(message);
        }
    }
}
