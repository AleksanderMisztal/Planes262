using GameDataStructures.Messages.Client;

namespace Planes262.Networking
{
    public interface IPacketSender
    {
        void SendData(ClientMessage message);
    }
}