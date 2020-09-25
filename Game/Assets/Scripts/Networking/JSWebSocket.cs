using System.Runtime.InteropServices;
using GameDataStructures.Packets;
using UnityEngine;

namespace Planes262.Networking
{
    public class JsWebSocket : MonoBehaviour, IPacketSender
    {
        private ServerTranslator serverTranslator = null;
#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")] private static extern void InitializeConnectionJs();
        [DllImport("__Internal")] private static extern void SendDataJs(string data);
#else
        private static void InitializeConnectionJs() { }
        private static void SendDataJs(string data) { }
#endif
        
        
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
            InitializeConnectionJs();
        }

        public void SendData(Packet packet)
        {
            string data = Serializer.Serialize(packet.ToArray());
            SendDataJs(data);
        }
    }
}