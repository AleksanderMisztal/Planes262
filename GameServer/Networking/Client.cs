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

        private bool isRunning;
        private readonly BlockingCollection<Packet> sendQueue = new BlockingCollection<Packet>(new ConcurrentQueue<Packet>());

        public event Action<int> ConnectionTerminated;

        public Client(int id, WebSocket socket, ServerHandle serverHandle)
        {
            this.id = id;
            this.socket = socket;
            this.serverHandle = serverHandle;

            isRunning = true;
            Task.Run(async () =>
            {
                while (isRunning) await BeginReceive();
            });
            Task.Run(async () =>
            {
                while (isRunning)
                {
                    Packet packet = sendQueue.Take();
                    await SendData(packet);
                }
            });
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
                Console.WriteLine($"Exception occured: {ex}. Disconnecting client {id}.");
                TerminateConnection();
                return;
            }
            
            if (data == null) return;

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

            memoryStream.Seek(0, SeekOrigin.Begin);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                //TerminateConnection();
                return null;
            }
            using StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        private void TerminateConnection()
        {
            ConnectionTerminated?.Invoke(id);
            isRunning = false;
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
