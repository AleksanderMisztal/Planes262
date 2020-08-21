using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameManager : MonoBehaviour
    {
        private ClientSend sender;
        private void Start()
        {
            CsWebSocket socket = new CsWebSocket();
            sender = new ClientSend(socket);

            UIManager.SetSender(sender);
            MapController.SetSender(sender);
            Messenger.SetSender(sender);
        }
    }
}