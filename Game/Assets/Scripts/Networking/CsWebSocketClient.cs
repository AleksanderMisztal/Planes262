using System;
using System.IO;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Planes262.Networking.Packets;

namespace Planes262.Networking
{
    public class CsWebSocketClient
    {
        private const string Host = "wss://localhost:5001";
        private readonly Queue<Packet> sendQueue = new Queue<Packet>();
        private ClientWebSocket socket;
        private readonly ClientHandle clientHandle;

        public CsWebSocketClient(ClientHandle clientHandle)
        {
            this.clientHandle = clientHandle;
        }


        public async Task InitializeConnection()
        {
            Uri serverUri = new Uri(Host);
            socket = new ClientWebSocket();
            Debug.Log("Attempting to connect to " + serverUri);
            await socket.ConnectAsync(serverUri, CancellationToken.None);
        }

        public async Task BeginListenAsync()
        {
            while (true)
            {
                string data = await Receive();
                clientHandle.HandlePacket(data);
            }
        }

        private async Task<string> Receive()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4 * 1024]);
            MemoryStream memoryStream = new MemoryStream();
            WebSocketReceiveResult result;

            do {
                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
            } while (!result.EndOfMessage);

            memoryStream.Seek(0, SeekOrigin.Begin);

            if (result.MessageType == WebSocketMessageType.Close)
                throw new Exception("Something went wrong while reading the message.");
            using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
            {
                string bytes = reader.ReadToEnd();
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

        private async Task SendData(Packet packet)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(Serializer.Serialize(packet.ToArray())));
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
        
        
        public void AddToQueue(Packet packet)
        {
            Packet perm = new Packet(packet.ToArray());
            sendQueue.Enqueue(perm);
        }

        public async Task BeginSendAsync()
        {
            while (true)
            {
                while (sendQueue.Count != 0)
                {
                    Packet packet = sendQueue.Dequeue();
                    await SendData(packet);
                }
                await Task.Delay(100);
            }
        }
    }
}