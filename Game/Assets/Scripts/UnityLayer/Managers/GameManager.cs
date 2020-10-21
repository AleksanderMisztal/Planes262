using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Planes262.GameLogic;
using Planes262.UnityLayer.HexSystem;
using Planes262.Utils;
using UnityEngine;

namespace Planes262.UnityLayer.Managers
{
    public class GameManager : MonoBehaviour
    {
        private TroopManager troopManager;
        private MapController mapController;
        private GridBase gridBase;
        private TroopInstantiator troopInstantiator;

        public Action<MoveAttemptEventArgs> MoveAttempted { private get; set; }

        private void Start()
        {
            gridBase = new GridBase(Board.Test, 1);
            troopInstantiator = FindObjectOfType<TroopInstantiator>();
            
            TroopMap troopMap = new TroopMap();
            troopManager = new TroopManager(troopMap);
            mapController = new MapController(gridBase, troopMap, args => MoveAttempted(args));

            InputParser inputParser = FindObjectOfType<InputParser>();
            inputParser.gridBase = gridBase;
            inputParser.CellClicked += cell => mapController.OnCellClicked(cell);

            FindObjectOfType<BoardCamera>().gridBase = gridBase;

            UnityTroopDecorator.effects = FindObjectOfType<Effects>();
            UnityTroopDecorator.gridBase = gridBase;
        }

        public void SetLocal(bool local)
        {
            mapController.IsLocal = local;
        }

        public void StartNewGame(Board board, PlayerSide side)
        {
            troopManager.ResetForNewGame();
            mapController.ResetForNewGame(side, board);
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
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
            gridBase.ResetAllTiles();
        }
    }
}