using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Planes262.GameLogic;
using Planes262.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer.Managers
{
    public class GameManager : MonoBehaviour
    {
        private TroopManager troopManager;
        private MapController mapController;
        private TileManager tileManager;
        private TroopInstantiator troopInstantiator;

        public Action<MoveAttemptEventArgs> MoveAttempted { private get; set; }

        private void Start()
        {
            tileManager = FindObjectOfType<TileManager>();
            troopInstantiator = FindObjectOfType<TroopInstantiator>();
            
            TroopMap troopMap = new TroopMap();
            troopManager = new TroopManager(troopMap);
            mapController = new MapController(tileManager, troopMap, args => MoveAttempted(args));

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
            troopManager.ResetForNewGame();
            mapController.ResetForNewGame(side, board);
            tileManager.CreateBoard(board);
        }

        public void BeginNextRound(IEnumerable<TroopDto> troops)
        {
            mapController.ToggleActivePlayer();
            IEnumerable<ITroop> uTroops = troops.Select(t => troopInstantiator.InstantiateTroop(t));
            troopManager.BeginNextRound(uTroops);
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            troopManager.MoveTroop(position, direction, battleResults);
        }

        public void OnGameEnded()
        {
            tileManager.DeactivateTiles();
        }
    }
}