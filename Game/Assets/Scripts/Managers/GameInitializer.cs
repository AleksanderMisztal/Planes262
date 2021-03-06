﻿using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Messages.Server;
using GameJudge;
using Planes262.HexSystem;
using Planes262.LevelEditor;
using Planes262.Networking;
using Planes262.UnityLayer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Planes262.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Material lineMaterial;
        [SerializeField] private Button endRoundButton;
        
        private GameManager gameManager;
        private ScoreDisplay score;
        private ClockDisplay clockDisplay;
        
        private GameEventsHandler geHandler;

        private static bool isLocal = true;
        private static GameJoinedMessage level;
        private PlayerSide activePlayer = PlayerSide.Blue;

        public static void LoadGame(GameJoinedMessage gjm, bool local)
        {
            level = gjm;
            isLocal = local;
            SceneManager.LoadScene("Board");
        }
        
        private void Awake()
        {
            MyLogger.myLogger = new UnityLogger();
            HexTile.lineMaterial = lineMaterial;
            
            gameManager = FindObjectOfType<GameManager>();
            clockDisplay = FindObjectOfType<ClockDisplay>();
            score = FindObjectOfType<ScoreDisplay>();
            geHandler = new GameEventsHandler(gameManager, score, clockDisplay);
            
            if (Client.instance != null)
                Client.instance.serverEvents.geHandler = geHandler;
        }

        private void Start()
        {
            if (isLocal) InitializeLocalGame(level);
            else InitializeOnlineGame(level);
        }

        public void InitializeOnlineGame(GameJoinedMessage gjm)
        {
            CameraDto cameraDto = gjm.levelDto.cameraDto;
            BackgroundManager backgroundManager = FindObjectOfType<BackgroundManager>();
            backgroundManager.SetBackground(gjm.levelDto.background);
            FindObjectOfType<BoardCamera>().Initialize(cameraDto);
            backgroundManager.DetachBackground();
            gameManager.Initialize(gjm.levelDto.board.Get());
            
            endRoundButton.onClick.AddListener(Client.instance.EndRound);
            
            gameManager.SetLocal(false);
            gameManager.MoveAttempted = args => Client.instance.MoveTroop(args.Position, args.Direction);
            
            geHandler.OnGameReady(gjm.opponentName, gjm.side, gjm.levelDto, gjm.clockInfo);
        }

        private void InitializeLocalGame(GameJoinedMessage gjm)
        {
            CameraDto cameraDto = gjm.levelDto.cameraDto;
            BackgroundManager backgroundManager = FindObjectOfType<BackgroundManager>();
            backgroundManager.SetBackground(gjm.levelDto.background);
            FindObjectOfType<BoardCamera>().Initialize(cameraDto);
            backgroundManager.DetachBackground();
            Board board = gjm.levelDto.board.Get();
            gameManager.Initialize(board);
            gameManager.SetLocal(true);

            WaveProvider waveProvider = new WaveProvider(gjm.levelDto.troopDtos);
            GameController gc = new GameController(waveProvider, board);
            Clock clock = new Clock(1000, 5, geHandler.OnLostOnTime);
            
            endRoundButton.onClick.AddListener(() => gc.EndRound(activePlayer));
            
            gc.TroopMoved += args => geHandler.OnTroopMoved(args.position, args.direction, args.battleResults, args.scoreInfo);
            gc.TroopsSpawned += troops =>
            {
                activePlayer = activePlayer.Opponent();
                TimeInfo ti = clock.ToggleActivePlayer();
                geHandler.OnTroopsSpawned(troops, ti);
            };
            gc.GameEnded += args => geHandler.OnGameEnded(args.scoreInfo);

            gameManager.MoveAttempted = args => gc.ProcessMove(args.Side, args.Position, args.Direction);
            
            ClockInfo clockInfo = clock.Initialize();
            geHandler.OnGameReady(gjm.opponentName, gjm.side, gjm.levelDto, clockInfo);
        }

        public void BackToMain()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}