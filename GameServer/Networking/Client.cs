using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using GameDataStructures.Messages;
using GameDataStructures.Messages.Client;
using GameDataStructures.Messages.Server;
using GameServer.Utils;

namespace GameServer.Networking
{
    public class Client
    {
        private readonly int id;
        private readonly WebSocket socket;
        private readonly ServerHandle serverHandle;

        public event Action<int> ConnectionTerminated;

        private bool isRunning = true;
        private readonly BlockingCollection<ServerMessage> sendQueue = new BlockingCollection<ServerMessage>(new ConcurrentQueue<ServerMessage>());
        
        public Client(int id, WebSocket socket, ServerHandle serverHandle)
        {
            this.id = id;
            this.socket = socket;
            this.serverHandle = serverHandle;
            
            Task.Run(async () =>
            {
                while (isRunning)
                {
                    ServerMessage message = sendQueue.Take();
                    await SendData(message);
                }
            });
        }

        public async Task Connect()
        {
            while (isRunning) await BeginReceive();
        }

        private async Task BeginReceive()
        {
            MemoryStream data;
            try
            {
                data = await Receive();
            }
            catch (WebSocketException ex)
            {
                TerminateConnection(ex.Message);
                return;
            }

            if (data == null)
            {
                TerminateConnection("disconnected.");
                return;
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                ClientMessage message = (ClientMessage) Serializer.DeserializeFromStream(data);
                serverHandle.Handle(id, message);
            });
        }

        private async Task<MemoryStream> Receive()
        {
            ArraySegment<byte> buffer = new(new byte[4 * 1024]);
            MemoryStream memoryStream = new();
            WebSocketReceiveResult result;

            do
            {
                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
            } while (!result.EndOfMessage);

            return result.MessageType == WebSocketMessageType.Close ? null : memoryStream;
        }

        private void TerminateConnection(string message)
        {
            Console.WriteLine($"Connection for client {id} terminated: " + message);
            isRunning = false;
            ConnectionTerminated?.Invoke(id);
        }

        public void Send(ServerMessage message)
        {
            sendQueue.Add(message);
        }
        
        private async Task SendData(ServerMessage message)
        {
            ArraySegment<byte> buffer = Serializer.SerializeToStream(message).GetBuffer();
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
    }
}
