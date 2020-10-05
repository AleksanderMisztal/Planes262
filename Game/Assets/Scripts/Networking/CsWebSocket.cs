using System;
using System.IO;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameDataStructures.Packets;
using UnityEngine;

namespace Planes262.Networking
{
    public class CsWebSocket : IPacketSender
    {
        private const string Host = "wss://localhost:5001";
        private readonly Queue<Packet> sendQueue = new Queue<Packet>();
        private ClientWebSocket socket;
        private readonly ServerTranslator serverTranslator;

        public CsWebSocket(ServerTranslator serverTranslator)
        {
            this.serverTranslator = serverTranslator;
        }


        public async void InitializeConnection()
        {
            Uri serverUri = new Uri(Host);
            socket = new ClientWebSocket();
            Debug.Log("Attempting to connect to " + serverUri);
            try
            {
                await socket.ConnectAsync(serverUri, CancellationToken.None);
                BeginSendAsync();
                await BeginListenAsync();
            }
            catch (Exception ex)
            {
                Debug.Log("Couldn't connect to server: " + ex.Message);
            }
        }

        private async Task BeginListenAsync()
        {
            while (true)
            {
                string data = await Receive();
                serverTranslator.HandlePacket(data);
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

        private async Task SendFromQueue(Packet packet)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(packet.Data));
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
        
        
        public void SendData(Packet packet)
        {
            sendQueue.Enqueue(packet);
        }

        private async Task BeginSendAsync()
        {
            while (true)
            {
                while (sendQueue.Count != 0)
                {
                    Packet packet = sendQueue.Dequeue();
                    await SendFromQueue(packet);
                }
                await Task.Delay(100);
            }
        }
    }
}