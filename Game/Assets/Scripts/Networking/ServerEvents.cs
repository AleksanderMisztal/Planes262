﻿using System;
using GameDataStructures.Messages.Server;
using Planes262.Managers;
using UnityEngine;

namespace Planes262.Networking
{
    public class ServerEvents
    {
        public GameEventsHandler geHandler;

        public event Action<string[]> OnWelcome;
        public event Action<string> OnMessageSent;


        public void HandlePacket(ServerMessage message)
        {
            switch (message.type)
            {
                case ServerPackets.Welcome:
                    WelcomeMessage wm = (WelcomeMessage) message;
                    Debug.Log("Connected to the server! Game types: " + string.Join(", ", wm.gameTypes));
                    OnWelcome?.Invoke(wm.gameTypes);
                    break;
                case ServerPackets.GameJoined:
                    GameJoinedMessage gjm = (GameJoinedMessage) message;
                    GameInitializer.LoadGame(gjm, false);
                    break;
                case ServerPackets.TroopsSpawned:
                    TroopsSpawnedMessage tsm = (TroopsSpawnedMessage) message;
                    geHandler.OnTroopsSpawned(tsm.troops, tsm.timeInfo);
                    break;
                case ServerPackets.TroopMoved:
                    TroopMovedMessage tmm = (TroopMovedMessage) message;
                    geHandler.OnTroopMoved(tmm.position, tmm.direction, tmm.battleResults, tmm.scoreInfo);
                    break;
                case ServerPackets.GameEnded:
                    GameEndedMessage gem = (GameEndedMessage) message;
                    geHandler.OnGameEnded(gem.scoreInfo);
                    break;
                case ServerPackets.OpponentDisconnected:
                    geHandler.OnOpponentDisconnected();
                    break;
                case ServerPackets.ChatSent:
                    ChatSentMessage csm = (ChatSentMessage) message;
                    OnMessageSent?.Invoke(csm.message);
                    break;
                case ServerPackets.LostOnTime:
                    LostOnTimeMessage lotm = (LostOnTimeMessage) message;
                    geHandler.OnLostOnTime(lotm.loser);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
