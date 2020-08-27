using Planes262.Networking;
using Planes262.UnityLayer.Utils;
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
        private TroopController troopController;
        private MapGrid mapGrid;

        private async void Awake()
        {
            mapController = new MapController();
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            tileManager = FindObjectOfType<TileManager>();
            troopController = FindObjectOfType<TroopController>();
            mapGrid = FindObjectOfType<MapGrid>();
            game = new Game(mapController, messenger, uiManager, troopController);
            ClientHandle clientHandle = new ClientHandle(game);
            CsWebSocket socket = new CsWebSocket(clientHandle);
            sender = new ClientSend(socket);


            await socket.InitializeConnection();
            await socket.BeginListenAsync();
        }

        private void Start()
        {
            mapController.Inject(sender, tileManager);
            uiManager.Inject(sender, messenger, tileManager);
            messenger.SetSender(sender);
            FindObjectOfType<InputParser>().Inject(mapController, mapGrid);
            GdTroop.Inject(mapGrid);
        }
    }
}