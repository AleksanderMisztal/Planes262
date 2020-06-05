using System;
using System.Linq;
using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.GameLogic
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        [SerializeField]
        private Troop blueTroopPrefab;
        [SerializeField]
        private Troop redTroopPrefab;

        //TODO: Get fight results from the server
        //TODO: Get waves from the server

        private PlayerId activePlayer = PlayerId.Red;
        private int roundNumber = 0;
        private int blueScore = 0;
        private int redScore = 0;

        private int movePointsLeft;
        
        private PlayerId Oponent => activePlayer == PlayerId.Red ? PlayerId.Blue : PlayerId.Red;

        private readonly HashSet<Troop> blueTroops = new HashSet<Troop>();
        private readonly HashSet<Troop> redTroops = new HashSet<Troop>();
        private readonly Dictionary<Vector2Int, Troop> troopAtPosition = new Dictionary<Vector2Int, Troop>();

        private Troop activeTroop = null;


        private void Awake()
        {
            Debug.Log("Game controller awake...");
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }


        // Public interface
        public void OnCellClicked(Vector2Int cell)
        {
            troopAtPosition.TryGetValue(cell, out Troop clickedTroop);

            if (activeTroop 
                && activeTroop.MovePoints > 0
                && (!clickedTroop || clickedTroop.ControllingPlayer == Oponent)
                && Hex.IsLegalMove(activeTroop.Position, activeTroop.Orientation, cell, out int direction))
            {
                MoveTroop(activeTroop, direction);
                return;
            }

            if (clickedTroop 
                && clickedTroop.ControllingPlayer == activePlayer
                && clickedTroop.MovePoints > 0)
            {
                SetActiveTroop(clickedTroop);
                return;
            }
            activeTroop?.Desactivate();
            activeTroop = null;
        }

        public void OnTroopMoved(Vector2Int position, int direction, List<BattleResult> battleResults)
        {
            Troop troop = troopAtPosition[position];
            Vector2Int targetPosition = troop.GetAdjacentHex(direction);
            if (!troopAtPosition.TryGetValue(targetPosition, out Troop encounter))
            {
                ChangeTroopPosition(position, targetPosition);
                troop.MoveInDirection(direction);
                DecrementMovePointsLeft();
                return;
            }

            troop.MoveInDirection(direction);

            int battleIndex = 0;

            ApplyDamage(battleResults[battleIndex++], troop, encounter);

            targetPosition = troop.GetAdjacentHex(0);
            troop.JumpForward();

            while (troopAtPosition.TryGetValue(targetPosition, out encounter))
            {
                ApplyDamage(battleResults[battleIndex++], troop, encounter);

                targetPosition = troop.GetAdjacentHex(0);
                troop.JumpForward();
            }

            ChangeTroopPosition(position, targetPosition);

            DecrementMovePointsLeft();
        }

        public void StartNextRound(IEnumerable<SpawnTemplate> spawns)
        {
            activeTroop?.Desactivate();
            activeTroop = null;

            roundNumber++;
            SpawnNextWave(spawns);

            if (activePlayer == PlayerId.Blue)
            {
                foreach (Troop troop in redTroops)
                {
                    troop.OnTurnBegin();
                }
                foreach (Troop troop in blueTroops)
                {
                    troop.OnTurnEnd();
                }
                activePlayer = PlayerId.Red;
            }
            else
            {
                foreach (Troop troop in blueTroops)
                {
                    troop.OnTurnBegin();
                }
                foreach (Troop troop in redTroops)
                {
                    troop.OnTurnEnd();
                }
                activePlayer = PlayerId.Blue;
            }

            SetInitialMovePointsLeft(activePlayer);
        }


        // Private functions
        private void SetActiveTroop(Troop troop)
        {
            activeTroop?.Desactivate();
            activeTroop = troop;
            activeTroop.Activate();
        }

        private void MoveTroop(Troop troop, int direction)
        {
            if (activePlayer != troop.ControllingPlayer)
            {
                throw new IllegalMoveException("Attempting to make a move in oponent's turn!");
            }

            if (troop.MovePoints <= 0)
            {
                throw new IllegalMoveException("Attempting to move a NewTroop with no move points!");
            }

            Vector2Int targetPosition = troop.GetAdjacentHex(direction);

            if (troopAtPosition.TryGetValue(targetPosition, out Troop encounter) && encounter.ControllingPlayer == troop.ControllingPlayer)
            {
                throw new IllegalMoveException("Attempting to enter a cell with friendly NewTroop!");
            }

            Vector2Int originalPosition = troop.Position;

            NetworkingHub.SendTroopMove(originalPosition, direction);
        }

        private void ApplyDamage(BattleResult battleResult, Troop attacker, Troop defender)
        {
            if (battleResult.AttackerDamaged)
            {
                attacker.ApplyDamage();
                if (attacker.Health <= 0)
                {
                    DestroyTroop(attacker);
                }

                IncrementScore(Oponent);
            }
            if (battleResult.DefenderDamaged)
            {
                defender.ApplyDamage();
                if (defender.Health <= 0)
                {
                    DestroyTroop(defender);
                }

                IncrementScore(activePlayer);
            }
        }

        private void IncrementScore(PlayerId player)
        {
            if (player == PlayerId.Blue) blueScore++;
            if (player == PlayerId.Red) redScore++;
            Debug.Log($"{blueScore} : {redScore}");
        }

        private void DestroyTroop(Troop NewTroop)
        {
            troopAtPosition.Remove(NewTroop.Position);

            //TODO: Only end the game if no more troops will be spawned

            if (NewTroop.ControllingPlayer == PlayerId.Blue)
            {
                blueTroops.Remove(NewTroop);
                if (blueTroops.Count == 0) EndGame();
            }
            else
            {
                redTroops.Remove(NewTroop);
                if (redTroops.Count == 0) EndGame();
            }

            if (NewTroop.ControllingPlayer == activePlayer)
            {
                movePointsLeft -= NewTroop.MovePoints;
            }
        }

        private void DecrementMovePointsLeft()
        {
            movePointsLeft--;
            Debug.Log($"Decrementing move points, {movePointsLeft} left.");
        }

        private void ChangeTroopPosition(Vector2Int oldPosition, Vector2Int newPosition)
        {
            troopAtPosition.Add(newPosition, troopAtPosition[oldPosition]);
            troopAtPosition.Remove(oldPosition);
        }

        private void SpawnNextWave(IEnumerable<SpawnTemplate> spawns)
        {
            foreach (SpawnTemplate spawn in spawns)
            {
                Troop troopPrefab = spawn.controllingPlayer == PlayerId.Blue ?
                    blueTroopPrefab : redTroopPrefab;

                Troop troop = Instantiate(troopPrefab);
                troop.Initilalize(spawn);

                Console.WriteLine($"Spawning troop at {spawn.position}");
                troopAtPosition.Add(spawn.position, troop);

                if (troop.ControllingPlayer == PlayerId.Blue)
                {
                    blueTroops.Add(troop);
                }
                else
                {
                    redTroops.Add(troop);
                }
            }
        }

        private void SetInitialMovePointsLeft(PlayerId player)
        {
            HashSet<Troop> troops = player == PlayerId.Blue ? blueTroops : redTroops;
            movePointsLeft = troops.Aggregate(0, (acc, t) => acc + t.InitialMovePoints);
        }

        private void EndGame()
        {
            Debug.Log($"Blue score: {blueScore}, red score: {redScore}");
        }
    }
}
