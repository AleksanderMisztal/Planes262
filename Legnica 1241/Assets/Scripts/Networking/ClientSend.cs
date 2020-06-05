using UnityEngine;

namespace Scripts.Networking
{
    public class ClientSend
    {
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.tcp.SendData(packet);
        }


        public static void WelcomeReceived()
        {
            using (Packet packet = new Packet((int)ClientPackets.WelcomeReceived))
            {
                packet.Write(Client.instance.myId);
                packet.Write("Elo");

                SendTcpData(packet);
            }
        }

        public static void MoveTroop(Vector2Int position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                SendTcpData(packet);
            }
        }
    }
}
