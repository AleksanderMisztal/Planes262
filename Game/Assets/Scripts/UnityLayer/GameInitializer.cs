using System.Threading.Tasks;
using Planes262.GameLogic;
using Planes262.Networking;
using Planes262.UnityLayer.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class GameInitializer : MonoBehaviour
    {
        private UIManager uiManager;
        private Messenger messenger;
        
        private GameManager gameManager;
        private MapController mapController;
        
        private ClientSend sender;
        private CsWebSocket socket;

        private async void Awake()
        {
            GetObjectsFromScene();
            InitializeGame();
            InitializeNetworking();
            InjectSender();
            InjectRest();
            await ConnectAsync();
        }

        private void GetObjectsFromScene()
        {
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
        }

        private void InitializeGame()
        {
            TileManager tileManager = FindObjectOfType<TileManager>();
            TroopInstantiator troopInstantiator = FindObjectOfType<TroopInstantiator>();
            Text scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();

            Score score = new UnityScore(scoreText);
            TroopMap troopMap = new TroopMap();
            UnityTroopManager unityTroopManager = new UnityTroopManager(troopMap, troopInstantiator, score);
            mapController = new MapController(tileManager, troopMap);
            Game game = new Game(unityTroopManager, mapController);

            gameManager = new GameManager(messenger, uiManager, game, tileManager);
        }

        private void InitializeNetworking()
        {
            ClientHandle clientHandle = new ClientHandle(gameManager);
            socket = new CsWebSocket(clientHandle);
            sender = new ClientSend(socket);
        }

        private void InjectSender()
        {
            mapController.Inject(sender);
            uiManager.Inject(sender);
            messenger.Inject(sender);
        }

        private void InjectRest()
        {
            MapGrid mapGrid = FindObjectOfType<MapGrid>();
            InputParser inputParser = FindObjectOfType<InputParser>();
            Effects effects = FindObjectOfType<Effects>();

            inputParser.Inject(mapController, mapGrid);
            UnityTroopDecorator.effects = effects;
            UnityTroopDecorator.mapGrid = mapGrid;
        }

        private async Task ConnectAsync()
        {
            await socket.InitializeConnection();
            await socket.BeginListenAsync();
        }
    }
}