using System.Threading.Tasks;
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

        private async void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            gameManager = FindObjectOfType<GameManager>();

            await InitializeServerConnection();
        }

        private async Task InitializeServerConnection()
        {
            ServerHandler serverHandler = new ServerHandler(messenger, uiManager, gameManager);
            
            ServerTranslator serverTranslator = new ServerTranslator(serverHandler);
            CsWebSocketClient wsClient = new CsWebSocketClient(serverTranslator);
            Client client = new Client(wsClient);
            
            //TODO: do this on online game started
            gameManager.MoveAttemptedHandler = args => client.MoveTroop(args.Position, args.Direction);
            uiManager.GameJoined += username => client.JoinGame(username);
            messenger.MessageSent += message => client.SendMessage(message);
            
            await wsClient.InitializeConnection();
            await wsClient.BeginListenAsync();
        }

        public void InitializeLocalGame()
        {
            GameController gameController = new GameController(Waves.Test(), Board.Test);
            
            gameController.TroopMoved += args => gameManager.MoveTroop(args.Position, args.Direction, args.BattleResults);
            gameController.TroopsSpawned += args => gameManager.BeginNextRound(args.Troops.ToUTroop());
            gameController.GameEnded += args => uiManager.EndGame(args.Score.ToString(), 1.5f);

            gameManager.MoveAttemptedHandler = args => gameController.ProcessMove(args.Side, args.Position, args.Direction);
            
            gameManager.SetLocal(true);
            gameManager.StartNewGame(Board.Test, PlayerSide.Blue);
            gameController.BeginGame();
            uiManager.TransitionIntoGame(Board.Test);
        }
    }
}