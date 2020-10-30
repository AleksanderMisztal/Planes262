// ReSharper disable once RedundantUsingDirective
using System.Runtime.InteropServices;
using GameDataStructures.Packets;
using UnityEngine;

namespace Planes262.Networking
{
    public class JsWebSocket : MonoBehaviour, IPacketSender
    {
        private ServerEvents serverEvents = null;
#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")] private static extern void InitializeConnectionJs();
        [DllImport("__Internal")] private static extern void SendDataJs(string data);
#else
        private static void InitializeConnectionJs() { }
        private static void SendDataJs(string data) { }
#endif
        
        
        public void SetEvents(ServerEvents events)
        {
            serverEvents = events;
        }

        public void ReceiveWsMessage(string byteArray)
        {
            serverEvents.HandlePacket(byteArray);
        }
        
        public void InitializeConnection()
        {
            InitializeConnectionJs();
        }

        public void SendData(Packet packet)
        {
            SendDataJs(packet.Data);
        }

        public void Close()
        {
            
        }
    }
}