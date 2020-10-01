using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Packets;
using Planes262.GameLogic.Troops;
using Planes262.UnityLayer.Managers;
using UnityEngine;

namespace Planes262.Networking
{
    public class ServerTranslator
    {
        private delegate void PacketHandler(Packet packet);
        private readonly Dictionary<int, PacketHandler> packetHandlers;
        private readonly ServerHandler serverHandler;

        public ServerTranslator(ServerHandler serverHandler)
        {
            this.serverHandler = serverHandler;
            packetHandlers = new Dictionary<int, PacketHandler>
            {
                {(int) ServerPackets.Welcome, Welcome },
                {(int) ServerPackets.GameJoined, GameJoined },
                {(int) ServerPackets.TroopSpawned, TroopSpawned },
                {(int) ServerPackets.TroopMoved, TroopMoved },
                {(int) ServerPackets.GameEnded, GameEnded },
                {(int) ServerPackets.OpponentDisconnected, OpponentDisconnected },
                {(int) ServerPackets.MessageSent, MessageSent },
                {(int) ServerPackets.LostOnTime, LostOnTime },
            };
        }

        
        public void HandlePacket(string byteArray)
        {
            byte[] bytes = Serializer.Deserialize(byteArray);
            using (Packet packet = new Packet(bytes))
            {
                int packetType = packet.ReadInt();
                packetHandlers[packetType](packet);
            }
        }


        private void Welcome(Packet packet)
        {
            serverHandler.OnWelcome();
        }

        private void GameJoined(Packet packet)
        {
            string opponentName = packet.ReadString();
            PlayerSide side = (PlayerSide)packet.ReadInt();
            Board board = packet.ReadBoard();

            serverHandler.OnGameJoined(opponentName, side, board);
        }

        private void TroopSpawned(Packet packet)
        {
            List<TroopDto> troopDtos = packet.ReadTroops();
            TimeInfo timeInfo = packet.ReadTimeInfo();

            serverHandler.OnTroopSpawned(troopDtos, timeInfo);
        }

        private void TroopMoved(Packet packet)
        {
            VectorTwo position = packet.ReadVector2Int();
            int direction = packet.ReadInt();
            List<BattleResult> battleResults = packet.ReadBattleResults();

            serverHandler.OnTroopMoved(position, direction, battleResults);
        }

        private void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            serverHandler.OnGameEnded(redScore, blueScore);
        }

        private void MessageSent(Packet packet)
        {
            string message = packet.ReadString();

            serverHandler.OnMessageSent(message);
        }

        private void OpponentDisconnected(Packet packet)
        {
            serverHandler.OnOpponentDisconnected();
        }

        private void LostOnTime(Packet packet)
        {
            PlayerSide loser = (PlayerSide)packet.ReadInt();

            serverHandler.OnLostOnTime(loser);
        }
    }
}
