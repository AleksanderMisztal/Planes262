using UnityEngine;
using Scripts.Networking;
using System.Runtime.InteropServices;

public class JSHandle : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void InitializeConnectionJS();

    public void ReceiveWsMessage(string message)
    {
        ClientHandle.HandlePacket(message);
    }

    private void Start()
    {
        InitializeConnectionJS();
    }
}
