using GameDataStructures;
using GameJudge;
using GameJudge.Waves;
using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        private Messenger messenger;
        private GameManager gameManager;
        private ScoreDisplay score;
        private ClockDisplay clockDisplay;
        
        private GameEventsHandler geHandler;

        private void Awake()
        {
            Debug.Log("Game initializer awaken");
            MyLogger.myLogger = new UnityLogger();
            messenger = FindObjectOfType<Messenger>();
            gameManager = FindObjectOfType<GameManager>();
            clockDisplay = FindObjectOfType<ClockDisplay>();
            score = FindObjectOfType<ScoreDisplay>();
            
            geHandler = new GameEventsHandler(gameManager, score, clockDisplay);

            InitializeServerConnection();
        }

        private void Start()
        {
            gameManager.Initialize();
            if (TransitionManager.isLocal) 
                InitializeLocalGame();
            else
                InitializeOnlineGame();
        }

        private void InitializeServerConnection()
        {
            messenger.MessageSent += message => Client.instance.SendAMessage(message);
        }

        public void InitializeOnlineGame()
        {
            gameManager.SetLocal(false);
            gameManager.MoveAttempted = args => Client.instance.MoveTroop(args.Position, args.Direction);
        }

        public void InitializeLocalGame()
        {            
            Debug.Log("Initializing a local game");
            gameManager.SetLocal(true);
            
            GameController gc = new GameController(WaveProvider.Test(), Board.test);
            Clock clock = new Clock(100, 5, geHandler.OnLostOnTime);
            
            gc.TroopMoved += args => geHandler.OnTroopMoved(args.Position, args.Direction, args.BattleResults, args.Score);
            gc.TroopsSpawned += args => {
                TimeInfo ti = clock.ToggleActivePlayer();
                geHandler.OnTroopSpawned(args.Troops, ti);
            };
            gc.GameEnded += args => geHandler.OnGameEnded(args.Score.Red, args.Score.Blue);

            gameManager.MoveAttempted = args => gc.ProcessMove(args.Side, args.Position, args.Direction);
            
            ClockInfo clockInfo = clock.Initialize();
            geHandler.OnGameJoined("local", PlayerSide.Blue, Board.test, clockInfo);
            gc.BeginGame();
        }
    }
}