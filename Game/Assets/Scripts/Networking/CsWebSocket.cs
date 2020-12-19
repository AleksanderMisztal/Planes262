using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures.Messages;
using GameDataStructures.Messages.Client;
using GameDataStructures.Messages.Server;
using UnityEngine;

namespace Planes262.Networking
{
    public class CsWebSocket : IPacketSender
    {
        private const string host = "wss://localhost:5001";
        private readonly Queue<ClientMessage> sendQueue = new Queue<ClientMessage>();
        private ClientWebSocket socket;
        private readonly ServerEvents serverEvents;

        public CsWebSocket(ServerEvents serverEvents)
        {
            this.serverEvents = serverEvents;
        }

        public async void InitializeConnection()
        {
            Uri serverUri = new Uri(host);
            socket = new ClientWebSocket();
            Debug.Log("Attempting to connect to " + serverUri);
            try
            {
                await socket.ConnectAsync(serverUri, CancellationToken.None);
                BeginSendAsync();
                await BeginListenAsync();
            }
            catch (WebSocketException ex)
            {
                Debug.Log("Couldn't connect to server: " + ex.Message);
            }
        }

        private async Task BeginListenAsync()
        {
            while (true)
            {
                ServerMessage message = await Receive();
                serverEvents.HandlePacket(message);
            }
        }

        private async Task<ServerMessage> Receive()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4 * 1024]);
            MemoryStream memoryStream = new MemoryStream();
            WebSocketReceiveResult result;

            do {
                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
            } while (!result.EndOfMessage);

            memoryStream.Seek(0, SeekOrigin.Begin);

            if (result.MessageType == WebSocketMessageType.Close) return null;
            Debug.Log(string.Join(",", memoryStream.GetBuffer()));
            return (ServerMessage) Serializer.DeserializeFromStream(memoryStream);
        }

        private async Task SendFromQueue(ClientMessage message)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Serializer.SerializeToStream(message).GetBuffer());
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
        
        
        public void SendData(ClientMessage packet)
        {
            sendQueue.Enqueue(packet);
        }

        private async Task BeginSendAsync()
        {
            while (true)
            {
                while (sendQueue.Count != 0)
                {
                    ClientMessage message = sendQueue.Dequeue();
                    await SendFromQueue(message);
                }
                await Task.Delay(100);
            }
        }

        public void Close()
        {
            if (socket.State == WebSocketState.Open)
                socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }
    }
}