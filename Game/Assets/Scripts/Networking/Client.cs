using GameDataStructures;
using Planes262.Networking.Packets;

namespace Planes262.Networking
{
    public class Client
    {
        private readonly CsWebSocketClient client;
        
        public Client(CsWebSocketClient client)
        {
            this.client = client;
            client.BeginSendAsync();
        }

        
        public void JoinGame(string name)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(name);

                client.AddToQueue(packet);
            }
        }

        public void MoveTroop(VectorTwo position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                client.AddToQueue(packet);
            }
        }

        public void SendMessage(string message)
        {
            using (Packet packet = new Packet((int)ClientPackets.SendMessage))
            {
                packet.Write(message);

                client.AddToQueue(packet);
            }
        }
    }
}
