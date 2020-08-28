using Planes262.GameLogic;
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
        private Effects effects;

        private async void Awake()
        {
            GetObjectsFromScene();
            
            TroopMap troopMap = new TroopMap();
            UnityTroopManager unityTroopManager = new UnityTroopManager(troopMap, troopInstantiator);
            MapController mapController = new MapController(tileManager, troopMap);
            Game game = new Game(unityTroopManager, mapController);
            
            GameManager gameManager = new GameManager(messenger, uiManager, game);
            
            ClientHandle clientHandle = new ClientHandle(gameManager);
            CsWebSocket socket = new CsWebSocket(clientHandle);
            ClientSend sender = new ClientSend(socket);

            mapController.Inject(sender);
            inputParser.Inject(mapController, mapGrid);
            uiManager.Inject(sender, messenger, tileManager);
            messenger.Inject(sender);
            UnityTroop.Inject(effects);
            TroopGO.Inject(mapGrid);

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
            effects = FindObjectOfType<Effects>();
        }
    }
}