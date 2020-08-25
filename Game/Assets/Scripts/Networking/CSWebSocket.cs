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
        private const string Host = "localhost:5001";
        
        private WsClient wsClient;

        public async Task InitializeConnection()
        {
            wsClient = new WsClient();
            await wsClient.Connect();
        }

        private class WsClient
        {
            private ClientWebSocket socket;

            public async Task Connect()
            {
                Uri serverUri = new Uri($"wss://{Host}");
                socket = new ClientWebSocket();
                Debug.Log("Attempting to connect to " + serverUri);
                await socket.ConnectAsync(serverUri, CancellationToken.None);

                //TODO: Fix this dangerous thingy
                await BeginListen();
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

            private async Task BeginListen()
            {
                while (true)
                {
                    Debug.Log("Received sth");
                    string data = await Receive();
                    ClientHandle.HandlePacket(data);
                }
            }

            private async Task SendData(Packet packet)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(Serializer.Serialize(packet.ToArray())));
                await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
    }
}