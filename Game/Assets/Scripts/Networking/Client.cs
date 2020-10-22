using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using Planes262.UnityLayer;
using UnityEngine;

namespace Planes262.Networking
{
    public class Client : MonoBehaviour
    {
        [HideInInspector] public static Client instance;

        private void Awake()
        {
            Debug.Log("Awakening the client");
            if (instance == this) return;
            if (instance == null)
            {
                Debug.Log("Setting instance to this.");
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
        public ServerEvents serverEvents;

        private void Start()
        {
            serverEvents = new ServerEvents();
#if UNITY_EDITOR || !UNITY_WEBGL
            CsWebSocket ws = new CsWebSocket(serverEvents);
            ws.InitializeConnection();
#else
            JsWebSocket ws = Instantiate(new GameObject().AddComponent<JsWebSocket>());
            ws.gameObject.name = "JsWebSocket";
            ws.SetTranslator(serverTranslator);
            ws.InitializeConnection();
#endif
            packetSender = ws;
        }

        
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
