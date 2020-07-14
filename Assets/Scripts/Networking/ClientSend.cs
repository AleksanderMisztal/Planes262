using UnityEngine;

namespace Scripts.Networking
{
    public static class ClientSend
    {
        public static void JoinLobby(string username)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinLobby))
            {
                //TODO: remove on both sides, client doesn't need to know their id
                packet.Write(-1);
                packet.Write(username);

                Client.SendData(packet);
            }
        }

        public static void JoinGame(int oponentId)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(oponentId);

                Client.SendData(packet);
            }
        }

        public static void MoveTroop(Vector2Int position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                Client.SendData(packet);
            }
        }

        public static void SendAMessage(string message)
        {
            using (Packet packet = new Packet((int)ClientPackets.SendMessage))
            {
                packet.Write(message);

                Client.SendData(packet);
            }
        }
    }
}
