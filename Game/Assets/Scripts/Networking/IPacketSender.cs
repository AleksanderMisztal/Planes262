
using GameDataStructures.Packets;

namespace Planes262.Networking
{
    public interface IPacketSender
    {
        void SendData(Packet packet);
    }
}