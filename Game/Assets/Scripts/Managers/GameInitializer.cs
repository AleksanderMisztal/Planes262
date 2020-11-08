using System.Collections;
using GameDataStructures;
using GameJudge;
using GameJudge.Waves;
using Planes262.HexSystem;
using Planes262.Networking;
using Planes262.Saving;
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
        private static string levelName = "level1";

        public static void LoadBoard(string aLevelName, bool local)
        {
            levelName = aLevelName;
            isLocal = local;
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
            if (isLocal) InitializeLocalGame();
            else InitializeOnlineGame();
        }

        private void InitializeBackground(BoardDto boardDto)
        {
            BackgroundManager backgroundManager = FindObjectOfType<BackgroundManager>();
            backgroundManager.SetBackground(boardDto.background);
            FindObjectOfType<BoardCamera>().Initialize(boardDto.offset, boardDto.ortoSize);
            StartCoroutine(DetachBackground(backgroundManager));
        }

        private IEnumerator DetachBackground(BackgroundManager bm)
        {
            yield return null;
            bm.DetachFromCamera();
        }

        private void InitializeOnlineGame()
        {
            BoardDto boardDto = Saver.Read<BoardDto>(levelName + "/board");
            InitializeBackground(boardDto);
            Board board = new Board(boardDto.xSize, boardDto.ySize);
            gameManager.Initialize(board);
            
            gameManager.SetLocal(false);
            gameManager.MoveAttempted = args => Client.instance.MoveTroop(args.Position, args.Direction);
        }

        private void InitializeLocalGame()
        {
            BoardDto boardDto = Saver.Read<BoardDto>(levelName + "/board");
            InitializeBackground(boardDto);
            Board board = new Board(boardDto.xSize, boardDto.ySize);
            TroopDto[] dtos = TroopLoader.Load(levelName);
            gameManager.Initialize(board);
            gameManager.SetLocal(true);

            WaveProvider waveProvider = new WaveProvider(dtos);
            GameController gc = new GameController(waveProvider, board);
            Clock clock = new Clock(1000, 5, geHandler.OnLostOnTime);
            
            gc.TroopMoved += args => geHandler.OnTroopMoved(args.position, args.direction, args.battleResults.ToArray(), args.score);
            gc.TroopsSpawned += troops => {
                TimeInfo ti = clock.ToggleActivePlayer();
                geHandler.OnTroopsSpawned(troops, ti);
            };
            gc.GameEnded += args => geHandler.OnGameEnded(args.score);

            gameManager.MoveAttempted = args => gc.ProcessMove(args.Side, args.Position, args.Direction);
            
            ClockInfo clockInfo = clock.Initialize();
            geHandler.OnGameReady("p2", PlayerSide.Blue, board, waveProvider.initialTroops, clockInfo);
        }
    }
}