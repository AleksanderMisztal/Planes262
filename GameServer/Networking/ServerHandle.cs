using System.Collections.Generic;
using GameDataStructures.Messages.Client;
using GameServer.Matchmaking;

namespace GameServer.Networking
{
    public class ServerHandle
    {
        private delegate void PacketHandler(int fromClient, ClientMessage packet);
        
        private readonly GameHandler gameHandler;
        private readonly Dictionary<ClientPackets, PacketHandler> packetHandlers;

        public ServerHandle(GameHandler gameHandler)
        {
            this.gameHandler = gameHandler;
            packetHandlers = new Dictionary<ClientPackets, PacketHandler>
            {
                {ClientPackets.JoinGame, JoinGame },
                {ClientPackets.MoveTroop, MoveTroop },
                {ClientPackets.SendMessage, SendMessage },
            };
        }

        public void Handle(int fromClient, ClientMessage message)
        {
            packetHandlers[message.type](fromClient, message);
        }
        
        private void JoinGame(int fromClient, ClientMessage message)
        {
            JoinGameMessage m = (JoinGameMessage) message;
            User newUser = new User(fromClient, m.username);
            gameHandler.SendToGame(newUser, m.gameType);
        }

        private void MoveTroop(int fromClient, ClientMessage message)
        {
            MoveTroopMessage m = (MoveTroopMessage) message;

            gameHandler.MoveTroop(fromClient, m.position, m.direction);
        }

        private void SendMessage(int fromClient, ClientMessage message)
        {
            SendChatMessage m = (SendChatMessage) message;

            gameHandler.SendMessage(fromClient, m.message);
        }
    }
}
