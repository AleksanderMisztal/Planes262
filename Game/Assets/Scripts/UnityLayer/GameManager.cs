using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameManager : MonoBehaviour
    {
        private MapController mapController;
        private ClientSend sender;
        private Game game;
        private Messenger messenger;

        private async void Awake()
        {
            mapController = new MapController();
            messenger = FindObjectOfType<Messenger>();
            game = new Game(mapController, messenger);
            ClientHandle clientHandle = new ClientHandle(game);
            CsWebSocket socket = new CsWebSocket(clientHandle);
            sender = new ClientSend(socket);

            FindObjectOfType<InputParser>().SetMapController(mapController);

            await socket.InitializeConnection();
            await socket.BeginListenAsync();
        }

        private void Start()
        {
            mapController.SetSender(sender);
            UIManager.Instance.SetSender(sender);
            UIManager.Instance.SetMessenger(messenger);
            messenger.SetSender(sender);
        }
    }
}