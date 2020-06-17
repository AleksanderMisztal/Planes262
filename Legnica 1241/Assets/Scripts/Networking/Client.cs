using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client instance;

        string host = "wwsserver.azurewebsites.net";
        //string host = "localhost";
        public int port = 443;
        public int myId;
        public WsClient wsClient;

        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Debug.Log("Client = this...");
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }

        private async void Start()
        {
            InitializePacketHandlers();
            wsClient = new WsClient();
            await wsClient.Connect();
        }

        private void InitializePacketHandlers()
        {
            packetHandlers = new Dictionary<int, PacketHandler>
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome },
                {(int)ServerPackets.GameJoined, ClientHandle.GameJoined },
                {(int)ServerPackets.TroopSpawned, ClientHandle.TroopSpawned },
                {(int)ServerPackets.TroopMoved, ClientHandle.TroopMoved },
                {(int)ServerPackets.GameEnded, ClientHandle.GameEnded },
            };
            Debug.Log("Initialized client packet handlers.");
        }


        public class WsClient
        {
            public ClientWebSocket socket;

            public async Task Connect()
            {
                Uri serverUri = new Uri($"wss://{instance.host}:{instance.port}");
                socket = new ClientWebSocket();
                Debug.Log("Attempting to connect to " + serverUri.ToString());
                await socket.ConnectAsync(serverUri, CancellationToken.None);
                await BeginListen();
            }

            private async Task<byte[]> Receive()
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4 * 1024]);
                var memoryStream = new MemoryStream();
                WebSocketReceiveResult result;

                do {
                    result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                memoryStream.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Binary)
                {
                    return memoryStream.ToArray();
                }
                throw new Exception("Something went wrong while reading the message.");
            }

            private async Task BeginListen()
            {
                byte[] data = await Receive();

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(data))
                    {
                        int packetType = packet.ReadInt();
                        packetHandlers[packetType](packet);
                    }
                });

                await BeginListen();
            }

            public async Task SendData(Packet packet)
            {
                var buffer = new ArraySegment<byte>(packet.ToArray());
                await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
    }
}
