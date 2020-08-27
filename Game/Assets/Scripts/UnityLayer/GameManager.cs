using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameManager : MonoBehaviour
    {
        private ClientSend sender;
        private Game game;
        
        private async void Awake()
        {
            game = new Game();
            ClientHandle clientHandle = new ClientHandle(game);
            CsWebSocket socket = new CsWebSocket(clientHandle);
            await socket.InitializeConnection();
            sender = new ClientSend(socket);

            UIManager.Instance.SetSender(sender);
            MapController.SetSender(sender);
            Messenger.SetSender(sender);

            await socket.BeginListenAsync();
        }

        
    }
}