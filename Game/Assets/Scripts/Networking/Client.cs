using Planes262.Networking.Packets;
using UnityEngine;

namespace Planes262.Networking
{
    public class Client : MonoBehaviour
    {
        private static Client instance;
        private CsWebSocket socket;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }

        private async void Start()
        {
            Application.runInBackground = true;

            socket = new CsWebSocket();
#if UNITY_EDITOR || !UNITY_WEBGL 
            await 
#endif
            socket.InitializeConnection();
        }

        public static void SendData(Packet packet)
        {
            instance.socket.SendData(packet);
        }

        public void ReceiveWsMessage(string message)
        {
            ClientHandle.HandlePacket(message);
        }
    }
}
