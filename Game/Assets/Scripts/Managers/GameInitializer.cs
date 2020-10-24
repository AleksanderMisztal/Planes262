using GameDataStructures;
using GameJudge;
using GameJudge.Waves;
using Planes262.HexSystem;
using Planes262.Networking;
using Planes262.UnityLayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Planes262.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Material lineMaterial;
        private GameManager gameManager;
        private ScoreDisplay score;
        private ClockDisplay clockDisplay;
        
        private GameEventsHandler geHandler;

        private static bool isLocal = true;
        private static string levelName = "board";

        public static void LoadBoard(string aLevelName, bool aIsLocal)
        {
            levelName = aLevelName;
            isLocal = aIsLocal;
            SceneManager.LoadScene("Board");
        }
        
        private void Awake()
        {
            MyLogger.myLogger = new UnityLogger();
            gameManager = FindObjectOfType<GameManager>();
            clockDisplay = FindObjectOfType<ClockDisplay>();
            score = FindObjectOfType<ScoreDisplay>();
            
            geHandler = new GameEventsHandler(gameManager, score, clockDisplay);
            if (Client.instance != null)
                Client.instance.serverEvents.geHandler = geHandler;
            
            HexTile.lineMaterial = lineMaterial;
        }

        private void Start()
        {
            gameManager.Initialize(levelName);
            if (isLocal) 
                InitializeLocalGame();
            else
                InitializeOnlineGame();
        }

        private void InitializeOnlineGame()
        {
            gameManager.SetLocal(false);
            gameManager.MoveAttempted = args => Client.instance.MoveTroop(args.Position, args.Direction);
        }

        private void InitializeLocalGame()
        {            
            Debug.Log("Initializing a local game");
            gameManager.SetLocal(true);

            WaveProvider waveProvider = WaveProvider.Test();
            GameController gc = new GameController(waveProvider, Board.test);
            Clock clock = new Clock(100, 5, geHandler.OnLostOnTime);
            
            gc.TroopMoved += args => geHandler.OnTroopMoved(args.position, args.direction, args.battleResults, args.score);
            gc.TroopsSpawned += args => {
                TimeInfo ti = clock.ToggleActivePlayer();
                geHandler.OnTroopsSpawned(args.troops, ti);
            };
            gc.GameEnded += args => geHandler.OnGameEnded(args.score.Red, args.score.Blue);

            gameManager.MoveAttempted = args => gc.ProcessMove(args.Side, args.Position, args.Direction);
            
            ClockInfo clockInfo = clock.Initialize();
            geHandler.OnGameReady("local", PlayerSide.Blue, Board.test, waveProvider.initialTroops, clockInfo);
        }
    }
}