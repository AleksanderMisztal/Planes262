using Scripts.Utils;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Scripts.Networking
{
    public static class ClientSend
    {
        [DllImport("__Internal")]
        private static extern void SendDataJS(string data);

        private static void SendData(Packet packet)
        {
            string byteArray = Serializer.Serialize(packet.ToArray());
            Debug.Log("Sending " + byteArray);
            SendDataJS(byteArray);
        }


        public static void JoinLobby(string username)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinLobby))
            {
                //TODO: remove on both sides, client doesn't need to know their id
                packet.Write(-1);
                packet.Write(username);

                SendData(packet);
            }
        }

        public static void JoinGame(int oponentId)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(oponentId);

                SendData(packet);
            }
        }

        public static void MoveTroop(Vector2Int position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                SendData(packet);
            }
        }
    }
}
