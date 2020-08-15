using GameServer.Utils;

namespace Scripts.Networking
{
    public static class ClientSend
    {
        public static void JoinGame(string name)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(name);

                Client.SendData(packet);
            }
        }

        public static void MoveTroop(VectorTwo position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                Client.SendData(packet);
            }
        }

        public static void SendMessage(string message)
        {
            using (Packet packet = new Packet((int)ClientPackets.SendMessage))
            {
                packet.Write(message);

                Client.SendData(packet);
            }
        }
    }
}
