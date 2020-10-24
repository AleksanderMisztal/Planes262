using System;
using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Planes262.Managers;
using UnityEngine;

namespace Planes262.Networking
{
    public class ServerEvents
    {
        public GameEventsHandler geHandler;

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
            Debug.Log(data);
            Packet packet = new Packet(data);
            int packetType = packet.ReadInt();
            packetHandlers[packetType](packet);
        }


        public event Action OnWelcome;
        private void Welcome(Packet packet)
        {
            OnWelcome?.Invoke();
        }

        private void GameJoined(Packet packet)
        {
            string opponentName = packet.ReadString();
            PlayerSide side = (PlayerSide)packet.ReadInt();
            Board board = packet.Read<Board>();
            List<Fighter> troops = packet.ReadList<Fighter>();
            ClockInfo clockInfo = packet.Read<ClockInfo>();

            geHandler.OnGameReady(opponentName, side, board, troops, clockInfo);
        }

        private void TroopsSpawned(Packet packet)
        {
            List<Fighter> troops = packet.ReadList<Fighter>();
            TimeInfo timeInfo = packet.Read<TimeInfo>();

            geHandler.OnTroopsSpawned(troops, timeInfo);
        }

        private void TroopMoved(Packet packet)
        {
            VectorTwo position = packet.Read<VectorTwo>();
            int direction = packet.ReadInt();
            List<BattleResult> battleResults = packet.ReadList<BattleResult>();
            ScoreInfo score = packet.Read<ScoreInfo>();

            geHandler.OnTroopMoved(position, direction, battleResults, score);
        }

        private void GameEnded(Packet packet)
        {
            int redScore = packet.ReadInt();
            int blueScore = packet.ReadInt();

            geHandler.OnGameEnded(redScore, blueScore);
        }

        public event Action<string> OnMessageSent;
        private void MessageSent(Packet packet)
        {
            string message = packet.ReadString();

            OnMessageSent?.Invoke(message);
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
