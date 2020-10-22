using System;
using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace Planes262.Networking
{
    public class ServerEvents
    {
        private delegate void PacketHandler(Packet packet);
        private readonly Dictionary<int, PacketHandler> packetHandlers;

        public ServerEvents()
        {
            packetHandlers = new Dictionary<int, PacketHandler>
            {
                {(int) ServerPackets.Welcome, Welcome },
                {(int) ServerPackets.GameJoined, GameJoined },
                {(int) ServerPackets.TroopSpawned, TroopsSpawned },
                {(int) ServerPackets.TroopMoved, TroopMoved },
                {(int) ServerPackets.GameEnded, GameEnded },
                {(int) ServerPackets.OpponentDisconnected, OpponentDisconnected },
                {(int) ServerPackets.MessageSent, MessageSent },
                {(int) ServerPackets.LostOnTime, LostOnTime },
            };
        }
        
        public void HandlePacket(string data)
        {
            Packet packet = new Packet(data);
            int packetType = packet.ReadInt();
            packetHandlers[packetType](packet);
        }


        public event Action OnWelcome;
        private void Welcome(Packet packet)
        {
            OnWelcome?.Invoke();
        }

        public event Action<string, PlayerSide, Board, ClockInfo> OnGameJoined;
        private void GameJoined(Packet packet)
        {
            string opponentName = packet.ReadString();
            PlayerSide side = (PlayerSide)packet.ReadInt();
            Board board = packet.Read<Board>();
            ClockInfo clockInfo = packet.Read<ClockInfo>();

            OnGameJoined?.Invoke(opponentName, side, board, clockInfo);
        }

        public event Action<List<Fighter>, TimeInfo> OnTroopsSpawned;
        private void TroopsSpawned(Packet packet)
        {
            List<Fighter> troops = packet.ReadList<Fighter>();
            TimeInfo timeInfo = packet.Read<TimeInfo>();

            OnTroopsSpawned?.Invoke(troops, timeInfo);
        }

        public event Action<VectorTwo, int, List<BattleResult>, ScoreInfo> OnTroopMoved;
        private void TroopMoved(Packet packet)
        {
            VectorTwo position = packet.Read<VectorTwo>();
            int direction = packet.ReadInt();
            List<BattleResult> battleResults = packet.ReadList<BattleResult>();
            ScoreInfo score = packet.Read<ScoreInfo>();

            OnTroopMoved?.Invoke(position, direction, battleResults, score);
        }

        public event Action<int, int> OnGameEnded;
        private void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            OnGameEnded?.Invoke(redScore, blueScore);
        }

        public event Action<string> OnMessageSent;
        private void MessageSent(Packet packet)
        {
            string message = packet.ReadString();

            OnMessageSent?.Invoke(message);
        }

        public event Action OnOpponentDisconnected;
        private void OpponentDisconnected(Packet packet)
        {
            OnOpponentDisconnected?.Invoke();
        }

        public event Action<PlayerSide> OnLostOnTime;
        private void LostOnTime(Packet packet)
        {
            PlayerSide loser = (PlayerSide)packet.ReadInt();

            OnLostOnTime?.Invoke(loser);
        }
    }
}
