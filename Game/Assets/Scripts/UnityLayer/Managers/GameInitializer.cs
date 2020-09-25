using GameDataStructures;
using GameJudge;
using GameJudge.WavesN;
using Planes262.GameLogic.Troops;
using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        private UIManager uiManager;
        private Messenger messenger;
        private GameManager gameManager;
        private Clock clock;
        private Client client;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            gameManager = FindObjectOfType<GameManager>();
            clock = FindObjectOfType<Clock>();

            InitializeServerConnection();
        }

        private void InitializeServerConnection()
        {
            ServerHandler serverHandler = new ServerHandler(messenger, uiManager, gameManager, clock);
            ServerTranslator serverTranslator = new ServerTranslator(serverHandler);
            
            #if UNITY_EDITOR || !UNITY_WEBGL
            CsWebSocket ws = new CsWebSocket(serverTranslator);
            ws.InitializeConnection();
            #else
            JsWebSocket ws = Instantiate(new GameObject().AddComponent<JsWebSocket>());
            ws.gameObject.name = "JsWebSocket";
            ws.SetTranslator(serverTranslator);
            ws.InitializeConnection();
            #endif
            client = new Client(ws);

            uiManager.GameJoined += username => client.JoinGame(username);
            messenger.MessageSent += message => client.SendMessage(message);
        }

        public void InitializeOnlineGame()
        {
            gameManager.MoveAttemptedHandler = args => client.MoveTroop(args.Position, args.Direction);
            clock.LostOnTime = side => { };
        }

        public void InitializeLocalGame()
        {
            GameController gameController = new GameController(Waves.Test(), Board.Test);
            
            gameController.TroopMoved += args => gameManager.MoveTroop(args.Position, args.Direction, args.BattleResults);
            gameController.TroopsSpawned += args => clock.ToggleActivePlayer();
            gameController.TroopsSpawned += args => gameManager.BeginNextRound(args.Troops);
            gameController.GameEnded += args => uiManager.EndGame(args.Score.ToString(), 1.5f);

            gameManager.MoveAttemptedHandler = args => gameController.ProcessMove(args.Side, args.Position, args.Direction);
            clock.LostOnTime = side => uiManager.EndGame($"{side} lost on time", 1.5f);
            
            gameManager.SetLocal(true);
            gameManager.StartNewGame(Board.Test, PlayerSide.Blue);
            gameController.BeginGame();
            uiManager.TransitionIntoGame(Board.Test);
            clock.IsPlaying = true;
        }
    }
}