// ReSharper disable once RedundantUsingDirective
using System.Runtime.InteropServices;
using GameDataStructures.Messages;
using GameDataStructures.Messages.Client;
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
            //serverEvents.HandlePacket(byteArray);
        }
        
        public void InitializeConnection()
        {
            InitializeConnectionJs();
        }

        public void SendData(ClientMessage message)
        {
            byte[] data = Serializer.SerializeToStream(message).GetBuffer();
            //SendDataJs(data);
        }

        public void Close()
        {
            
        }
    }
}