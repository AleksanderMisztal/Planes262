using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using Planes262.UnityLayer.Managers;

namespace Planes262.Networking
{
    public class Client
    {
        private readonly IPacketSender packetSender;

        public Client(GameEventsHandler geHandler)
        {
            ServerTranslator translator = new ServerTranslator(geHandler);
#if UNITY_EDITOR || !UNITY_WEBGL
            CsWebSocket ws = new CsWebSocket(translator);
            ws.InitializeConnection();
#else
            JsWebSocket ws = Instantiate(new GameObject().AddComponent<JsWebSocket>());
            ws.gameObject.name = "JsWebSocket";
            ws.SetTranslator(serverTranslator);
            ws.InitializeConnection();
#endif
            packetSender = ws;
        }

        
        public void JoinGame(string name)
        {
            Packet packet = new Packet(ClientPackets.JoinGame);
            {
                packet.Write(name);

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

        public void SendMessage(string message)
        {
            Packet packet = new Packet(ClientPackets.SendMessage);
            {
                packet.Write(message);

                packetSender.SendData(packet);
            }
        }
    }
}
