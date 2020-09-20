using System;
using System.Collections.Generic;
using GameDataStructures;
using Planes262.GameLogic;
using Planes262.GameLogic.Troops;
using Planes262.UnityLayer.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer.Managers
{
    public class GameManager : MonoBehaviour
    {
        private UnityTroopManager unityTroopManager;
        private MapController mapController;
        private TileManager tileManager;

        public Action<MoveAttemptEventArgs> MoveAttemptedHandler { private get; set; }

        private void Start()
        {
            tileManager = FindObjectOfType<TileManager>();
            TroopInstantiator troopInstantiator = FindObjectOfType<TroopInstantiator>();

            Text scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();
            Score score = new UnityScore(scoreText);
            
            TroopMap troopMap = new TroopMap();
            unityTroopManager = new UnityTroopManager(troopMap, troopInstantiator, score);
            mapController = new MapController(tileManager, troopMap, args => MoveAttemptedHandler(args));

            FindObjectOfType<InputParser>().CellClicked += cell => mapController.OnCellClicked(cell);

            UnityTroopDecorator.effects = FindObjectOfType<Effects>();
            UnityTroopDecorator.mapGrid = FindObjectOfType<MapGrid>();
        }

        public void SetLocal(bool local)
        {
            mapController.IsLocal = local;
        }

        public void StartNewGame(Board board, PlayerSide side)
        {
            unityTroopManager.ResetForNewGame();
            mapController.ResetForNewGame(side, board);
            tileManager.CreateBoard(board);
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            mapController.ToggleActivePlayer();
            unityTroopManager.BeginNextRound(troops);
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            unityTroopManager.MoveTroop(position, direction, battleResults);
        }

        public void OnGameEnded()
        {
            tileManager.DeactivateTiles();
        }
    }
}