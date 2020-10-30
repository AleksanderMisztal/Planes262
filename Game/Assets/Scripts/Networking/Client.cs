using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using Planes262.UnityLayer;
using UnityEngine;

namespace Planes262.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client instance;

        private void Awake()
        {
            if (instance == this) return;
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Debug.Log("Destroying the client!");
                Destroy(this);
            }
        }

        private IPacketSender packetSender;
        public readonly ServerEvents serverEvents = new ServerEvents();

        private void Start()
        {
            serverEvents.OnWelcome += () => Debug.Log("Connected to server!");
#if UNITY_EDITOR || !UNITY_WEBGL
            ws = new CsWebSocket(serverEvents);
            ws.InitializeConnection();
#else
            JsWebSocket ws = Instantiate(new GameObject().AddComponent<JsWebSocket>());
            ws.gameObject.name = "JsWebSocket";
            ws.SetEvents(serverEvents);
            ws.InitializeConnection();
#endif
            packetSender = ws;
        }
#if UNITY_EDITOR || !UNITY_WEBGL
        private CsWebSocket ws;
        private void OnApplicationQuit()
        {
            ws.Close();
        }
#endif

        public void JoinGame()
        {
            Packet packet = new Packet(ClientPackets.JoinGame);
            {
                packet.Write(PlayerMeta.name);

                packetSender.SendData(packet);
            }
        }

        public void MoveTroop(VectorTwo position, int direction)
        {
            Packet packet = new Packet(ClientPackets.MoveTroop);
            {
                packet.Write(position);
                packet.Write(direction);

                packetSender.SendData(packet);
            }
        }

        public void SendAMessage(string message)
        {
            Packet packet = new Packet(ClientPackets.SendMessage);
            {
                packet.Write(message);

                packetSender.SendData(packet);
            }
        }
    }
}
