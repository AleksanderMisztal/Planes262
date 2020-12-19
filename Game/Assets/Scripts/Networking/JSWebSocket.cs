// ReSharper disable once RedundantUsingDirective
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using GameDataStructures.Messages;
using GameDataStructures.Messages.Client;
using GameDataStructures.Messages.Server;
using UnityEngine;

namespace Planes262.Networking
{
    public class JsWebSocket : MonoBehaviour, IPacketSender
    {
        private ServerEvents serverEvents = null;
#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")] private static extern void InitializeConnectionJs();
        [DllImport("__Internal")] private static extern void SendDataJs(byte[] array, int size);
#else
        private static void InitializeConnectionJs() { }
        private static void SendDataJs(byte[] array, int size) { }
#endif
        
        public void SetEvents(ServerEvents events)
        {
            serverEvents = events;
        }

        public void ReceiveWsMessage(string data)
        {
            byte[] bytes = data.Split(',').Select(byte.Parse).ToArray();
            ServerMessage m = (ServerMessage) Serializer.DeserializeFromStream(new MemoryStream(bytes));
            serverEvents.HandlePacket(m);
        }
        
        public void InitializeConnection()
        {
            InitializeConnectionJs();
        }

        public void SendData(ClientMessage message)
        {
            byte[] data = Serializer.SerializeToStream(message).GetBuffer();
            SendDataJs(data, data.Length);
        }

        public void Close()
        {
            
        }
    }
}