using System.Runtime.InteropServices;
using Planes262.Networking.Packets;

namespace Planes262.Networking
{
    public class JSWebSocket
    {
        [DllImport("__Internal")]
        public static extern void InitializeConnectionJS();

        public void InitializeConnection()
        {
            InitializeConnectionJS();
        }

        [DllImport("__Internal")]
        public static extern void SendDataJS(string data);

        public void SendData(Packet packet)
        {
            var data = Serializer.Serialize(packet.ToArray());
            SendDataJS(data);
        }
    }
}