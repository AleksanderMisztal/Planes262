using Planes262.Networking.Packets;

namespace Planes262.Networking
{
    public interface IPacketSender
    {
        void SendData(Packet packet);
    }
}