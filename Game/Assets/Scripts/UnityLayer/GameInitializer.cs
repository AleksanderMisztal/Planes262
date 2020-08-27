using Planes262.Networking;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameInitializer : MonoBehaviour
    {
        private TileManager tileManager;
        private MapGrid mapGrid;
        private InputParser inputParser;
        private TroopInstantiator troopInstantiator;
        private UIManager uiManager;
        private Messenger messenger;

        private async void Awake()
        {
            GetObjectsFromScene();
            
            MapController mapController = new MapController(tileManager);
            Game game = new Game(new UnityTroopController(troopInstantiator), mapController);
            GameManager gameManager = new GameManager(messenger, uiManager, game);
            
            ClientHandle clientHandle = new ClientHandle(gameManager);
            CsWebSocket socket = new CsWebSocket(clientHandle);
            ClientSend sender = new ClientSend(socket);

            mapController.Inject(sender);
            uiManager.Inject(sender, messenger, tileManager);
            messenger.Inject(sender);
            inputParser.Inject(mapController, mapGrid);
            UnityTroop.Inject(mapGrid);

            await socket.InitializeConnection();
            await socket.BeginListenAsync();
        }

        private void GetObjectsFromScene()
        {
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            tileManager = FindObjectOfType<TileManager>();
            mapGrid = FindObjectOfType<MapGrid>();
            inputParser = FindObjectOfType<InputParser>();
            troopInstantiator = FindObjectOfType<TroopInstantiator>();
        }
    }
}