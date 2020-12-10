using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using GameDataStructures.Messages.Server;
using GameJudge;
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
            sender.Welcome(nextClientId++, GameConfig.configs.Keys.ToArray());
            await client.Connect();
        }

        public void SendMessage(int toClient, ServerMessage message)
        {
            clients[toClient].Send(message);
        }
    }
}
