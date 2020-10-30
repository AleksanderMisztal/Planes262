using System;
using System.Collections.Generic;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using GameServer.Matchmaking;

namespace GameServer.Networking
{
    public class ServerHandle
    {
        private delegate void PacketHandler(int fromClient, Packet packet);
        
        private readonly GameHandler gameHandler;
        private readonly Dictionary<int, PacketHandler> packetHandlers;

        public ServerHandle(GameHandler gameHandler)
        {
            this.gameHandler = gameHandler;
            packetHandlers = new Dictionary<int, PacketHandler>
            {
                {(int) ClientPackets.JoinGame, JoinGame },
                {(int) ClientPackets.MoveTroop, MoveTroop },
                {(int) ClientPackets.SendMessage, SendMessage },
            };
        }

        public void Handle(int fromClient, Packet packet)
        {
            int packetType = packet.ReadInt();
            try
            {
                packetHandlers[packetType](fromClient, packet);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Unsupported packet type: " + packetType);
            }
        }        
        
        
        private void JoinGame(int fromClient, Packet packet)
        {
            string username = packet.ReadString();
            User newUser = new User(fromClient, username);
            gameHandler.SendToGame(newUser);
        }

        private void MoveTroop(int fromClient, Packet packet)
        {
            VectorTwo position = packet.Read<VectorTwo>();
            int direction = packet.ReadInt();

            gameHandler.MoveTroop(fromClient, position, direction);
        }

        private void SendMessage(int fromClient, Packet packet)
        {
            string message = packet.ReadString();

            gameHandler.SendMessage(fromClient, message);
        }
    }
}
