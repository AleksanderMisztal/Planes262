using UnityEngine;

namespace Scripts.Networking
{
    class Client : MonoBehaviour
    {
        private static Client instance;
        private WebSocket socket;

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
            socket = new WebSocket();
#if UNITY_EDITOR || !UNITY_WEBGL 
            await 
#endif
            socket.InitializeConnection();
        }

        public static void SendData(Packet packet)
        {
            instance.socket.SendData(packet);
        }

#if UNITY_EDITOR || !UNITY_WEBGL
        private async void FixedUpdate()
        {
            await socket.wsClient.SendPackets();
        }
#endif
    }
}
