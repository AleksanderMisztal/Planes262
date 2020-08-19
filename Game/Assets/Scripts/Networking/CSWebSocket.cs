#if UNITY_EDITOR || !UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Planes262.Networking.Packets;

namespace Planes262.Networking
{
    public class CsWebSocket
    {
        //private readonly static string host = "wwsserver.azurewebsites.net";
        private const string host = "localhost";
        private const int port = 5001;
        private WsClient wsClient;

        public async Task InitializeConnection()
        {
            wsClient = new WsClient();
            await wsClient.Connect();
        }

        public void SendData(Packet packet)
        {
            var perm = new Packet(packet.ToArray());
            wsClient.AddToQueue(perm);
        }

        private class WsClient
        {
            private readonly Queue<Packet> sendQueue = new Queue<Packet>();

            public void AddToQueue(Packet packet)
            {
                sendQueue.Enqueue(packet);
            }

            private async Task SendPackets()
            {
                while (true)
                {
                    while (sendQueue.Count != 0)
                    {
                        var packet = sendQueue.Dequeue();
                        await SendData(packet);
                    }
                    await Task.Delay(100);
                }
            }


            private ClientWebSocket socket;

            public async Task Connect()
            {
                var serverUri = new Uri($"wss://{host}:{port}");
                socket = new ClientWebSocket();
                Debug.Log("Attempting to connect to " + serverUri);
                await socket.ConnectAsync(serverUri, CancellationToken.None);

                //TODO: Fix this dangerous thingy
                SendPackets();
                await BeginListen();
            }

            private async Task<string> Receive()
            {
                var buffer = new ArraySegment<byte>(new byte[4 * 1024]);
                var memoryStream = new MemoryStream();
                WebSocketReceiveResult result;

                do {
                    result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                memoryStream.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Close)
                    throw new Exception("Something went wrong while reading the message.");
                using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    var bytes = reader.ReadToEnd();
                    try
                    {
                        return bytes;
                    }
                    catch
                    {
                        Debug.Log("Couldn't convert to bytes");
                    }
                }
                throw new Exception("Something went wrong while reading the message.");
            }

            private async Task BeginListen()
            {
                var data = await Receive();

                ClientHandle.HandlePacket(data);

                await BeginListen();
            }

            private async Task SendData(Packet packet)
            {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(Serializer.Serialize(packet.ToArray())));
                await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
    }
}
#endif