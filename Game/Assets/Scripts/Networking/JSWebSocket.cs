using System.Runtime.InteropServices;
using Planes262.Networking.Packets;
using UnityEngine;

namespace Planes262.Networking
{
    public class JsWebSocket : MonoBehaviour, IPacketSender
    {
        private ServerTranslator serverTranslator;
        
        [DllImport("__Internal")] private static extern void InitializeConnectionJS();
        [DllImport("__Internal")] private static extern void SendDataJS(string data);

        public void SetTranslator(ServerTranslator translator)
        {
            serverTranslator = translator;
        }

        public void ReceiveWsMessage(string byteArray)
        {
            serverTranslator.HandlePacket(byteArray);
        }
        
        public void InitializeConnection()
        {
            InitializeConnectionJS();
        }

        public void SendData(Packet packet)
        {
            string data = Serializer.Serialize(packet.ToArray());
            SendDataJS(data);
        }
    }
}