using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Planes262.GameLogic;
using Planes262.HexSystem;
using Planes262.Networking;
using Planes262.UnityLayer;
using Planes262.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Planes262.Managers
{
    public class GameManager : MonoBehaviour
    {
        private TroopManager troopManager;
        private MapController mapController;
        private GridBase gridBase;
        private HexInspector hexInspector;
        private TroopInstantiator troopInstantiator;

        public Action<MoveAttemptEventArgs> MoveAttempted { private get; set; }

        public void Initialize(string levelName)
        {
            gridBase = new GridBase(Board.test, 1);
            hexInspector = new HexInspector(Board.test, gridBase);
            troopInstantiator = FindObjectOfType<TroopInstantiator>();
            
            TroopMap troopMap = new TroopMap();
            troopManager = new TroopManager(troopMap);
            mapController = new MapController(gridBase, troopMap, args => MoveAttempted(args));

            InputParser inputParser = FindObjectOfType<InputParser>();
            inputParser.gridBase = gridBase;
            inputParser.CellClicked += mapController.OnCellClicked;
            inputParser.CellInspected += hexInspector.Inspect;

            FindObjectOfType<BoardCamera>().gridBase = gridBase;

            UnityTroopDecorator.effects = FindObjectOfType<Effects>();
            UnityTroopDecorator.gridBase = gridBase;

            LoadLevel(levelName);
        }

        private void LoadLevel(string levelName)
        {
            FindObjectOfType<BackgroundManager>().SetBackground(levelName);
        }

        public void SetLocal(bool local)
        {
            mapController.isLocal = local;
        }

        public void StartNewGame(Board board, IEnumerable<Troop> troops, PlayerSide side)
        {
            mapController.ResetForNewGame(side, board);
            IEnumerable<ITroop> uTroops = troops.Select(t => troopInstantiator.InstantiateTroop(t));
            troopManager.BeginNextRound(uTroops);
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

        public void EndGame(string message, float delay)
        {
            gridBase.ResetAllTiles();
            PersistState.gameEndedMessage = message;
            if (Client.instance != null) Client.instance.serverEvents.geHandler = null;
            StartCoroutine(DelayedSceneChange(delay));
        }

        private static IEnumerator DelayedSceneChange(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("Game Ended");
        }
    }
}