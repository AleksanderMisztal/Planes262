using System.Collections;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameJudge;
using GameJudge.Waves;
using Planes262.HexSystem;
using Planes262.LevelEditor;
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
        private static string levelName;

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
            else InitializeOnlineGame(null);
        }

        private void InitializeOnlineGame(LevelDto levelDto)
        {
            CameraDto cameraDto = levelDto.cameraDto;
            BackgroundManager backgroundManager = FindObjectOfType<BackgroundManager>();
            backgroundManager.SetBackground(levelDto.background);
            FindObjectOfType<BoardCamera>().Initialize(cameraDto);
            backgroundManager.DetachBackground();
            gameManager.Initialize(levelDto.board.Get());
            
            gameManager.SetLocal(false);
            gameManager.MoveAttempted = args => Client.instance.MoveTroop(args.Position, args.Direction);
        }

        private void InitializeLocalGame()
        {
            LevelDto levelDto = Saver.Read(levelName);
            CameraDto cameraDto = levelDto.cameraDto;
            BackgroundManager backgroundManager = FindObjectOfType<BackgroundManager>();
            backgroundManager.SetBackground(levelDto.background);
            FindObjectOfType<BoardCamera>().Initialize(cameraDto);
            backgroundManager.DetachBackground();
            Board board = levelDto.board.Get();
            gameManager.Initialize(board);
            gameManager.SetLocal(true);

            WaveProvider waveProvider = new WaveProvider(levelDto.troopDtos);
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