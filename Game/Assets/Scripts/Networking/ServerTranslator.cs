using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using Planes262.UnityLayer.Managers;

namespace Planes262.Networking
{
    public class ServerTranslator
    {
        private delegate void PacketHandler(Packet packet);
        private readonly Dictionary<int, PacketHandler> packetHandlers;
        private readonly GameEventsHandler geHandler;

        public ServerTranslator(GameEventsHandler geHandler)
        {
            this.geHandler = geHandler;
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
            geHandler.OnWelcome();
        }

        private void GameJoined(Packet packet)
        {
            string opponentName = packet.ReadString();
            PlayerSide side = (PlayerSide)packet.ReadInt();
            Board board = packet.ReadBoard();
            ClockInfo clockInfo = packet.ReadClockInfo();

            geHandler.OnGameJoined(opponentName, side, board, clockInfo);
        }

        private void TroopSpawned(Packet packet)
        {
            List<TroopDto> troopDtos = packet.ReadTroops();
            TimeInfo timeInfo = packet.ReadTimeInfo();

            geHandler.OnTroopSpawned(troopDtos, timeInfo);
        }

        private void TroopMoved(Packet packet)
        {
            VectorTwo position = packet.ReadVector2Int();
            int direction = packet.ReadInt();
            List<BattleResult> battleResults = packet.ReadBattleResults();

            geHandler.OnTroopMoved(position, direction, battleResults);
        }

        private void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            geHandler.OnGameEnded(redScore, blueScore);
        }

        private void MessageSent(Packet packet)
        {
            string message = packet.ReadString();

            geHandler.OnMessageSent(message);
        }

        private void OpponentDisconnected(Packet packet)
        {
            geHandler.OnOpponentDisconnected();
        }

        private void LostOnTime(Packet packet)
        {
            PlayerSide loser = (PlayerSide)packet.ReadInt();

            geHandler.OnLostOnTime(loser);
        }
    }
}
