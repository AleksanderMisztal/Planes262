using GameDataStructures;
using GameDataStructures.Packets;

namespace Planes262.Networking
{
    public class Client
    {
        private readonly IPacketSender packetSender;
        
        public Client(IPacketSender packetSender)
        {
            this.packetSender = packetSender;
        }

        
        public void JoinGame(string name)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(name);

                packetSender.SendData(packet);
            }
        }

        public void MoveTroop(VectorTwo position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                packetSender.SendData(packet);
            }
        }

        public void SendMessage(string message)
        {
            using (Packet packet = new Packet((int)ClientPackets.SendMessage))
            {
                packet.Write(message);

                packetSender.SendData(packet);
            }
        }
    }
}
