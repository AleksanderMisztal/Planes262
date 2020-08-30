using Planes262.GameLogic;
using Planes262.Networking;
using Planes262.UnityLayer.Utils;
using UnityEngine;
using UnityEngine.UI;

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
        private Text scoreText;

        private async void Awake()
        {
            GetObjectsFromScene();
            Score score = new UnityScore(scoreText);
            TroopMap troopMap = new TroopMap();
            UnityTroopManager unityTroopManager = new UnityTroopManager(troopMap, troopInstantiator, score);
            MapController mapController = new MapController(tileManager, troopMap);
            Game game = new Game(unityTroopManager, mapController);
            
            GameManager gameManager = new GameManager(messenger, uiManager, game, tileManager);
            
            ClientHandle clientHandle = new ClientHandle(gameManager);
            CsWebSocket socket = new CsWebSocket(clientHandle);
            ClientSend sender = new ClientSend(socket);

            mapController.Inject(sender);
            inputParser.Inject(mapController, mapGrid);
            uiManager.Inject(sender);
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
            scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();
        }
    }
}