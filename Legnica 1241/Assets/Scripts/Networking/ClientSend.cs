using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Networking
{
    public class ClientSend
    {
        private static async Task SendDataWs(Packet packet)
        {
            await Client.instance.wsClient.SendData(packet);
        }


        public static async Task JoinLobby(string username)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinLobby))
            {
                packet.Write(Client.instance.myId);
                packet.Write(username);

                await SendDataWs(packet);
            }
        }

        public static async Task JoinGame(int oponentId)
        {
            using (Packet packet = new Packet((int)ClientPackets.JoinGame))
            {
                packet.Write(oponentId);

                await SendDataWs(packet);
            }
        }

        public static async Task MoveTroop(Vector2Int position, int direction)
        {
            using (Packet packet = new Packet((int)ClientPackets.MoveTroop))
            {
                packet.Write(position);
                packet.Write(direction);

                await SendDataWs(packet);
            }
        }
    }
}
