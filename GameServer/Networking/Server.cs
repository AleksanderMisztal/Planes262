using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
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


        public async Task ConnectNewClient(WebSocket socket)
        {
            Console.WriteLine("Connecting a new client");

            Client client = new Client(nextClientId, socket, serverHandle);
            client.ConnectionTerminated += id => gameHandler.ClientDisconnected(id);

            clients.Add(nextClientId, client);
            nextClientId++;

            sender.Welcome(nextClientId - 1);
            await client.Connect();
        }

        public void SendPacket(int toClient, Packet packet)
        {
            clients[toClient].Send(packet);
        }
    }
}
