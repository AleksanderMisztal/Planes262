using System;
using System.Collections.Generic;
using System.Threading;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.GameLogic
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        public static PlayerId Side { get; set; }

        [SerializeField]
        private Troop blueTroopPrefab;
        [SerializeField]
        private Troop redTroopPrefab;

        private PlayerId activePlayer = PlayerId.Red;
        private int blueScore = 0;
        private int redScore = 0;
        
        private PlayerId Oponent => activePlayer == PlayerId.Red ? PlayerId.Blue : PlayerId.Red;

        private readonly HashSet<Troop> blueTroops = new HashSet<Troop>();
        private readonly HashSet<Troop> redTroops = new HashSet<Troop>();
        private readonly Dictionary<Vector2Int, Troop> troopAtPosition = new Dictionary<Vector2Int, Troop>();

        private Troop activeTroop = null;
        private HashSet<Vector2Int> activeTiles = new HashSet<Vector2Int>();
        private Dictionary<Vector2Int, Vector2Int> tileParent = new Dictionary<Vector2Int, Vector2Int>();

        private Stack<Vector2Int> path = null;
        private Vector2Int target = new Vector2Int();


        private void Awake()
        {
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
            if (activeTroop && activeTiles.Contains(cell))
            {
                if (path == null || cell != target)
                {
                    HighlightPathTo(cell);
                }
                else 
                {
                    MoveActiveTroop();
                }
                return;
            }

            troopAtPosition.TryGetValue(cell, out Troop clickedTroop);

            if (activeTroop 
                && activeTroop.MovePoints > 0
                && (!clickedTroop || clickedTroop.ControllingPlayer == Oponent)
                && Hex.IsLegalMove(activeTroop.Position, activeTroop.Orientation, cell, out  int direction))
            {
                MoveTroop(activeTroop, direction);
                return;
            }

            if (clickedTroop 
                && clickedTroop.ControllingPlayer == activePlayer
                && clickedTroop.ControllingPlayer == Side
                && clickedTroop.MovePoints > 0)
            {
                SetActiveTroop(clickedTroop);
                return;
            }
            activeTroop?.Deactivate();
            activeTroop = null;
        }

        public void OnTroopMoved(Vector2Int position, int direction, List<BattleResult> battleResults)
        {
            Troop troop = troopAtPosition[position];

            troop.MoveInDirection(direction);

            foreach (var battleResult in battleResults)
            {
                Troop encounter = troopAtPosition[troop.Position];
                ApplyDamage(battleResult, troop, encounter);
                troop.JumpForward();
            }

            if (troop.Health > 0)
            {
                ChangeTroopPosition(troop);
                if (troop.MovePoints <= 0 && activeTroop)
                {
                    troop.Deactivate();
                    activeTroop = null;
                }
            }
            SetActiveTiles();
            Thread.Sleep(400);
        }

        public void StartNextRound(IEnumerable<SpawnTemplate> spawns)
        {
            activeTroop?.Deactivate();
            activeTroop = null;

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
        }

        public void EndGame()
        {
            Debug.Log("Ending the game");
            foreach (Troop troop in troopAtPosition.Values)
            {
                Destroy(troop.gameObject);
            }
            troopAtPosition.Clear();
            blueTroops.Clear();
            redTroops.Clear();
            blueScore = 0;
            redScore = 0;
            activeTroop = null;
            // TODO: server should send starting player
            activePlayer = PlayerId.Red;
        }


        // Private functions
        private void HighlightPathTo(Vector2Int cell)
        {
            target = cell;
            path = new Stack<Vector2Int>();
            Vector2Int v = cell;
            path.Push(v);
            while (tileParent.TryGetValue(v, out v) && v != activeTroop.Position)
            {
                path.Push(v);
            }
            TileManager.HighlightPath(path);
        }

        private void MoveActiveTroop()
        {
            Hex coords = new Hex(activeTroop.Position, activeTroop.Orientation);
            while (path.Count > 0)
            {
                Vector2Int v = path.Pop();
                if (!Hex.IsLegalMove(coords.Position, coords.Orientation, v, out int dir))
                    throw new IllegalMoveException("bad dir");
                NetworkingHub.SendTroopMove(coords.Position, dir);
                coords.Move(dir);
            }
            path = null;
        }

        private void SetActiveTroop(Troop troop)
        {
            activeTroop?.Deactivate();
            activeTroop = troop;
            activeTroop.Activate();
            SetActiveTiles();
        }

        private void SetActiveTiles()
        {
            if (!activeTroop) return;
            tileParent = new Dictionary<Vector2Int, Vector2Int>();
            var tiles = GetReachableTiles(activeTroop.Position, activeTroop.Orientation, activeTroop.MovePoints);
            activeTiles = tiles;
            TileManager.ActivateTiles(tiles);
        }

        private HashSet<Vector2Int> GetReachableTiles(Vector2Int position, int orientation, int movePoints)
        {
            HashSet<Vector2Int> acm = new HashSet<Vector2Int>();
            Queue<Action> q = new Queue<Action>();

            q.Enqueue(() => GetReachableTiles(position, orientation, movePoints, acm, q));
            while(q.Count > 0)
            {
                q.Dequeue()();
            }
            return acm;
        }

        private void GetReachableTiles(Vector2Int position, int orientation, int movePoints, HashSet<Vector2Int> acm, Queue<Action> q)
        {
            if (movePoints <= 0) return;

            for (int i = -1; i < 2; i++)
            {
                int direction = (orientation + i + 6) % 6;
                Vector2Int tile = Hex.GetAdjacentHex(position, direction);

                if (troopAtPosition.TryGetValue(tile, out Troop enc) && enc.ControllingPlayer == activePlayer) continue;
                if (!acm.Contains(tile))
                {
                    acm.Add(tile);
                    tileParent.Add(tile, position);
                }
                if (!enc) q.Enqueue(() => GetReachableTiles(tile, direction, movePoints - 1, acm, q));
            }
        }

        private void MoveTroop(Troop troop, int direction)
        {
            direction = (direction + 6) % 6;
            if (direction != 0 && direction != 1 && direction != 5)
            {
                throw new IllegalMoveException("Troop can't move in direction " + direction + "!");
            }

            if (activePlayer != troop.ControllingPlayer)
            {
                throw new IllegalMoveException("Attempting to make a move in oponent's turn!");
            }

            if (troop.MovePoints <= 0)
            {
                throw new IllegalMoveException("Attempting to move a troop with no move points!");
            }

            Vector2Int targetPosition = troop.GetAdjacentHex(direction);

            if (troopAtPosition.TryGetValue(targetPosition, out Troop encounter) && encounter.ControllingPlayer == troop.ControllingPlayer)
            {
                throw new IllegalMoveException("Attempting to enter a cell with a friendly troop!");
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

        private void DestroyTroop(Troop troop)
        {
            troopAtPosition.Remove(troop.StartingPosition);
            if (activeTroop == troop)
            {
                activeTroop = null;
                TileManager.DeactivateTiles();
            }

            if (troop.ControllingPlayer == PlayerId.Blue)
            {
                blueTroops.Remove(troop);
            }
            else
            {
                redTroops.Remove(troop);
            }
        }

        private void ChangeTroopPosition(Troop troop)
        {
            troopAtPosition.Add(troop.Position, troop);
            troopAtPosition.Remove(troop.StartingPosition);

            troop.StartingPosition = troop.Position;
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

        //activate / deactivate troops
    }
}
