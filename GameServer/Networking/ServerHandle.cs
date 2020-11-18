using System;
using GameDataStructures.Messages.Client;
using GameServer.Matchmaking;

namespace GameServer.Networking
{
    public class ServerHandle
    {
        private readonly GameHandler gameHandler;

        public ServerHandle(GameHandler gameHandler)
        {
            this.gameHandler = gameHandler;
        }

        public void Handle(int fromClient, ClientMessage message)
        {
            switch (message.type)
            {
                case ClientPackets.JoinGame:
                    JoinGameMessage m = (JoinGameMessage) message;
                    User newUser = new User(fromClient, m.username);
                    gameHandler.SendToGame(newUser, m.gameType);
                    break;
                case ClientPackets.EndRound:
                    gameHandler.EndRound(fromClient);
                    break;
                case ClientPackets.MoveTroop:
                    MoveTroopMessage mtm = (MoveTroopMessage) message;
                    gameHandler.MoveTroop(fromClient, mtm.position, mtm.direction);
                    break;
                case ClientPackets.SendMessage:
                    SendChatMessage sm = (SendChatMessage) message;
                    gameHandler.SendMessage(fromClient, sm.message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
