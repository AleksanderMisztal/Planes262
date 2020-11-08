using System;
using System.Collections.Generic;
using GameDataStructures.Messages.Server;
using Planes262.Managers;
using UnityEngine;

namespace Planes262.Networking
{
    public class ServerEvents
    {
        public GameEventsHandler geHandler;

        private delegate void MessageHandler(ServerMessage message);
        private readonly Dictionary<ServerPackets, MessageHandler> packetHandlers;

        public ServerEvents()
        {
            packetHandlers = new Dictionary<ServerPackets, MessageHandler>
            {
                {ServerPackets.Welcome, Welcome },
                {ServerPackets.GameJoined, GameJoined },
                {ServerPackets.TroopsSpawned, TroopsSpawned },
                {ServerPackets.TroopMoved, TroopMoved },
                {ServerPackets.GameEnded, GameEnded },
                {ServerPackets.OpponentDisconnected, OpponentDisconnected },
                {ServerPackets.ChatSent, ChatSent },
                {ServerPackets.LostOnTime, LostOnTime },
            };
        }
        
        public void HandlePacket(ServerMessage message)
        {
            if (geHandler == null && message.type != ServerPackets.Welcome)
            {
                Debug.Log("This packet shouldn't have been sent");
                return;
            }
            packetHandlers[message.type](message);
        }


        public event Action<string[]> OnWelcome;
        private void Welcome(ServerMessage message)
        {
            WelcomeMessage m = (WelcomeMessage) message;
            Debug.Log("Connected to the server! Game types: " + string.Join(", ", m.gameTypes));
            OnWelcome?.Invoke(m.gameTypes);
        }

        private void GameJoined(ServerMessage message)
        {
            GameJoinedMessage m = (GameJoinedMessage) message;
            geHandler.OnGameReady(m.opponentName, m.side, m.board, m.troops, m.clockInfo);
        }

        private void TroopsSpawned(ServerMessage message)
        {
            TroopsSpawnedMessage m = (TroopsSpawnedMessage) message;
            geHandler.OnTroopsSpawned(m.troops, m.timeInfo);
        }

        private void TroopMoved(ServerMessage message)
        {
            TroopMovedMessage m = (TroopMovedMessage) message;
            geHandler.OnTroopMoved(m.position, m.direction, m.battleResults, m.scoreInfo);
        }

        private void GameEnded(ServerMessage message)
        {
            GameEndedMessage m = (GameEndedMessage) message;
            geHandler.OnGameEnded(m.scoreInfo);
        }

        public event Action<string> OnMessageSent;
        private void ChatSent(ServerMessage message)
        {
            ChatSentMessage m = (ChatSentMessage) message;
            OnMessageSent?.Invoke(m.message);
        }

        private void OpponentDisconnected(ServerMessage message)
        {
            geHandler.OnOpponentDisconnected();
        }

        private void LostOnTime(ServerMessage message)
        {
            LostOnTimeMessage m = (LostOnTimeMessage) message;
            geHandler.OnLostOnTime(m.loser);
        }
    }
}
