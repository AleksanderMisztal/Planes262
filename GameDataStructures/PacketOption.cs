using GameDataStructures.Packets;

namespace GameDataStructures
{
    public class PacketOption
    {
        private readonly Packet packet;

        public PacketOption()
        {
            packet = new Packet((int)ClientPackets.JoinGame);
        }

        public Packet GetPacket()
        {
            return packet;
        }
    }
}