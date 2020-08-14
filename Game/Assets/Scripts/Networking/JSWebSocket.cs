#if UNITY_WEBGL && !UNITY_EDITOR
using Scripts.Networking;
using Scripts.Utils;
using System.Runtime.InteropServices;

public class WebSocket
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
        string data = Serializer.Serialize(packet.ToArray());
        SendDataJS(data);
    }
}
#endif