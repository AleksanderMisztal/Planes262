using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using Planes262.UnityLayer;
using UnityEngine;

namespace Planes262.Networking
{
    public class Client : MonoBehaviour
    {
        private readonly IPacketSender packetSender;
        public readonly ServerEvents serverEvents;

        public Client()
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
