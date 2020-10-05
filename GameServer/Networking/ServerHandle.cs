using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using GameServer.Matchmaking;

namespace GameServer.Networking
{
    public class ServerHandle
    {
        private delegate Task PacketHandler(int fromClient, Packet packet);
        
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

        public async Task Handle(int fromClient, Packet packet)
        {
            int packetType = packet.Read<int>();
            try
            {
                await packetHandlers[packetType](fromClient, packet);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Unsupported packet type: " + packetType);
            }
        }        
        
        
        private async Task JoinGame(int fromClient, Packet packet)
        {
            string username = packet.Read<string>();
            User newUser = new User(fromClient, username);
            await gameHandler.SendToGame(newUser);
        }

        private Task MoveTroop(int fromClient, Packet packet)
        {
            VectorTwo position = packet.Read<VectorTwo>();
            int direction = packet.Read<int>();

            gameHandler.MoveTroop(fromClient, position, direction);
            return Task.CompletedTask;
        }

        private async Task SendMessage(int fromClient, Packet packet)
        {
            string message = packet.Read<string>();

            await gameHandler.SendMessage(fromClient, message);
        }
    }
}
