using GameDataStructures;
using GameJudge;
using GameJudge.WavesN;
using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        private UIManager uiManager;
        private Messenger messenger;
        private GameManager gameManager;
        private ClockDisplay clockDisplay;
        private Client client;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            gameManager = FindObjectOfType<GameManager>();
            clockDisplay = FindObjectOfType<ClockDisplay>();

            InitializeServerConnection();
        }

        private void InitializeServerConnection()
        {
            ServerHandler serverHandler = new ServerHandler(messenger, uiManager, gameManager, clockDisplay);
            ServerTranslator serverTranslator = new ServerTranslator(serverHandler);
            client = new Client(serverTranslator);

            uiManager.GameJoined += username => client.JoinGame(username);
            messenger.MessageSent += message => client.SendMessage(message);
        }

        public void InitializeOnlineGame()
        {
            gameManager.MoveAttemptedHandler = args => client.MoveTroop(args.Position, args.Direction);
            clockDisplay.ResetTime();
        }

        public void InitializeLocalGame()
        {
            GameController gameController = new GameController(Waves.Test(), Board.Test);
            Clock clock = new Clock(10, 5, loser =>
            {
                uiManager.EndGame($"Player {loser} lost on time :(", 0);
                gameManager.OnGameEnded();
            });
            
            gameController.TroopMoved += args => gameManager.MoveTroop(args.Position, args.Direction, args.BattleResults);
            //gameController.TroopsSpawned += args => clock.ToggleActivePlayer();
            gameController.TroopsSpawned += args => gameManager.BeginNextRound(args.Troops);
            gameController.GameEnded += args => uiManager.EndGame(args.Score.ToString(), 1.5f);

            gameManager.MoveAttemptedHandler = args => gameController.ProcessMove(args.Side, args.Position, args.Direction);
            
            gameManager.SetLocal(true);
            clockDisplay.ResetTime();
            gameManager.StartNewGame(Board.Test, PlayerSide.Blue);
            gameController.BeginGame();
            uiManager.TransitionIntoGame(Board.Test);
        }
    }
}