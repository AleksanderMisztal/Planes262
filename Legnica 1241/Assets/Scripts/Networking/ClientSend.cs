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


        public static void JoinLobby(string username)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinLobby))
            {
                packet.Write(Client.instance.myId);
                packet.Write(username);

                SendTcpData(packet);
            }
        }

        public static void JoinGame(int oponentId)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(oponentId);

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
