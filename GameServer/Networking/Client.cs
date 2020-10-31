using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameDataStructures.Packets;
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
        private readonly BlockingCollection<Packet> sendQueue = new BlockingCollection<Packet>(new ConcurrentQueue<Packet>());
        
        public Client(int id, WebSocket socket, ServerHandle serverHandle)
        {
            this.id = id;
            this.socket = socket;
            this.serverHandle = serverHandle;
            
            Task.Run(async () =>
            {
                while (isRunning)
                {
                    Packet packet = sendQueue.Take();
                    await SendData(packet);
                }
            });
        }

        public async Task Connect()
        {
            while (isRunning) await BeginReceive();
        }

        private async Task BeginReceive()
        {
            string data;
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
                Packet packet = new Packet(data);
                serverHandle.Handle(id, packet);
            });
        }

        private async Task<string> Receive()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4 * 1024]);
            MemoryStream memoryStream = new MemoryStream();
            WebSocketReceiveResult result;

            do
            {
                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
            } while (!result.EndOfMessage);

            if (result.MessageType == WebSocketMessageType.Close) return null;

            memoryStream.Seek(0, SeekOrigin.Begin);
            using StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        private void TerminateConnection(string message)
        {
            Console.WriteLine($"Connection for client {id} terminated: " + message);
            isRunning = false;
            ConnectionTerminated?.Invoke(id);
        }

        public void Send(Packet packet)
        {
            sendQueue.Add(packet);
        }
        
        private async Task SendData(Packet packet)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(packet.Data));
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
    }
}
