using System.Collections.Generic;
using System.Threading.Tasks;
using Planes262.GameLogic.Utils;
using Planes262.Networking.Packets;

namespace Planes262.Networking
{
    public class ClientSend
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
                    Packet packet = sendQueue.Dequeue();
                    await socket.SendData(packet);
                }
                await Task.Delay(100);
            }
        }

        private void SendData(Packet packet)
        {
            Packet perm = new Packet(packet.ToArray());
            AddToQueue(perm);
        }
        
        private readonly CsWebSocket socket;

        public ClientSend(CsWebSocket socket)
        {
            this.socket = socket;
        }

        public void JoinGame(string name)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(name);

                socket.SendData(packet);
            }
        }

        public void MoveTroop(VectorTwo position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                socket.SendData(packet);
            }
        }

        public void SendMessage(string message)
        {
            using (Packet packet = new Packet((int)ClientPackets.SendMessage))
            {
                packet.Write(message);

                socket.SendData(packet);
            }
        }
    }
}
