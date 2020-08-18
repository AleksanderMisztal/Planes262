using Planes262.GameLogic.Utils;
using Planes262.Networking.Packets;

namespace Planes262.Networking
{
    public static class ClientSend
    {
        public static void JoinGame(string name)
        {
            using (var packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(name);

                Client.SendData(packet);
            }
        }

        public static void MoveTroop(VectorTwo position, int direction)
        {
            using (var packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                Client.SendData(packet);
            }
        }

        public static void SendMessage(string message)
        {
            using (var packet = new Packet((int)ClientPackets.SendMessage))
            {
                packet.Write(message);

                Client.SendData(packet);
            }
        }
    }
}
