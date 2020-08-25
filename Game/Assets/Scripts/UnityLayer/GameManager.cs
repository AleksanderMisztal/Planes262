using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameManager : MonoBehaviour
    {
        private ClientSend sender;
            
        private async void Awake()
        {
            CsWebSocket socket = new CsWebSocket();
            await socket.InitializeConnection();
            sender = new ClientSend(socket);

            UIManager.SetSender(sender);
            MapController.SetSender(sender);
            Messenger.SetSender(sender);

            await socket.BeginListenAsync();
        }
    }
}