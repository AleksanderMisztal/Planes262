using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameManager : MonoBehaviour
    {
        private ClientSend sender;
        private Game game;
        private MapController mapController;
        private Messenger messenger;
        private UIManager uiManager;
        private TileManager tileManager;

        private async void Awake()
        {
            mapController = new MapController();
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            tileManager = FindObjectOfType<TileManager>();
            game = new Game(mapController, messenger, uiManager);
            ClientHandle clientHandle = new ClientHandle(game);
            CsWebSocket socket = new CsWebSocket(clientHandle);
            sender = new ClientSend(socket);

            FindObjectOfType<InputParser>().SetMapController(mapController);

            await socket.InitializeConnection();
            await socket.BeginListenAsync();
        }

        private void Start()
        {
            mapController.Inject(sender, tileManager);
            uiManager.Inject(sender, messenger, tileManager);
            messenger.SetSender(sender);
        }
    }
}