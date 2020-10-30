using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using GameDataStructures.Packets;
using GameServer.Matchmaking;

namespace GameServer.Networking
{
    public class Server
    {
        private readonly ServerSend sender;
        private readonly GameHandler gameHandler;
        private readonly ServerHandle serverHandle;

        public Server()
        {
            sender = new ServerSend(this);
            gameHandler = new GameHandler(sender);
            serverHandle = new ServerHandle(gameHandler);
        }

        private int nextClientId;
        private readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();


        public void ConnectNewClient(WebSocket socket)
        {
            Console.WriteLine("Connecting a new client");

            Client client = new Client(nextClientId, socket, serverHandle);

            clients.Add(nextClientId, client);
            nextClientId++;

            sender.Welcome(nextClientId - 1);
        }

        public void SendPacket(int toClient, Packet packet)
        {
            try
            {
                clients[toClient].Send(packet);
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine("Exception thrown by server while sending data: " + ex);
                gameHandler.ClientDisconnected(toClient);
            }
        }
    }
}
