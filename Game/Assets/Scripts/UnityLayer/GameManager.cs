using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameManager : MonoBehaviour
    {
        private ClientSend sender;
        private async void Start()
        {
            CsWebSocket socket = new CsWebSocket();
            await socket.InitializeConnection();
            sender = new ClientSend(socket);

            UIManager.SetSender(sender);
            MapController.SetSender(sender);
            Messenger.SetSender(sender);
        }
    }
}