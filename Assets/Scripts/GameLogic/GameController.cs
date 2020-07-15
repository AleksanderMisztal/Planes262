using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Scripts.UnityStuff;
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
        private Dictionary<Vector2Int, Vector3Int> addOrientation = new Dictionary<Vector2Int, Vector3Int>();
        private Dictionary<Vector3Int, Vector3Int> tileParent = new Dictionary<Vector3Int, Vector3Int>();

        private Stack<Vector2Int> path = null;
        private Vector2Int target = new Vector2Int();

        public static bool AcceptsCalls { get; private set; } = true;

        public static int BlueScore => instance.blueScore;
        public static int RedScore => instance.redScore;


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
            if (!AcceptsCalls) return;
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
            path = null;
        }

        public async UniTask OnTroopMoved(Vector2Int position, int direction, List<BattleResult> battleResults)
        {
            if (!AcceptsCalls) throw new Exception("Not acepting calls!");
            AcceptsCalls = false;

            Troop troop = troopAtPosition[position];

            await troop.MoveInDirection(direction);

            foreach (var battleResult in battleResults)
            {
                Troop encounter = troopAtPosition[troop.Position];
                ApplyDamage(battleResult, troop, encounter);
                await troop.JumpForward();
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

            AcceptsCalls = true;
        }

        public void StartNextRound(IEnumerable<SpawnTemplate> spawns)
        {
            if (!AcceptsCalls) throw new Exception("Not acepting calls!");
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
            if (!AcceptsCalls) throw new Exception("Not acepting calls!");
            Debug.Log("Ending the game");
            foreach (Troop troop in troopAtPosition.Values)
            {
                Destroy(troop.gameObject);
            }
            troopAtPosition.Clear();
            blueTroops.Clear();
            redTroops.Clear();
            activeTroop = null;
            path = null;
            blueScore = 0;
            redScore = 0;
            // TODO: server should send starting player
            activePlayer = PlayerId.Red;
        }


        // Private functions
        private bool BlockedByAllies(Troop troop)
        {
            foreach (var cell in Hex.GetControllZone(troop.Position, troop.Orientation))
            {
                if (!troopAtPosition.TryGetValue(cell, out Troop enc) || enc.ControllingPlayer != troop.ControllingPlayer)
                    return false;
            }
            return true;
        }

        private void HighlightPathTo(Vector2Int cell)
        {
            target = cell;
            path = new Stack<Vector2Int>();
            Vector3Int v = addOrientation[cell];
            path.Push((Vector2Int)v);
            while (tileParent.TryGetValue(v, out v) && (Vector2Int)v != activeTroop.Position)
            {
                path.Push((Vector2Int)v);
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
                {
                    Debug.Log("Illegal move!");
                    return;
                }
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
            tileParent = new Dictionary<Vector3Int, Vector3Int>();
            var tiles = GetReachableTiles(activeTroop.Position, activeTroop.Orientation, activeTroop.MovePoints);
            activeTiles = tiles;
            if (BlockedByAllies(activeTroop))
                TileManager.ActivateTilesBlocked(tiles);
            else
                TileManager.ActivateTiles(tiles);
        }

        private HashSet<Vector2Int> GetReachableTiles(Vector2Int position, int orientation, int movePoints)
        {
            bool blocked = Hex.GetControllZone(position, orientation)
                .All(c => troopAtPosition.TryGetValue(c, out Troop enc) && enc.ControllingPlayer == activePlayer);

            if (blocked) 
            {
                var cells = new HashSet<Vector2Int>();
                for (int i = -1; i < 2; i++)
                {
                    int direction = (orientation + i + 6) % 6;
                    Vector2Int tile = Hex.GetAdjacentHex(position, direction);
                    Vector3Int orientedTile = new Vector3Int(tile.x, tile.y, direction);
                    Vector3Int orientedParent = new Vector3Int(position.x, position.y, orientation);
                    cells.Add(tile);
                    addOrientation[tile] = orientedTile;
                    if (!tileParent.ContainsKey(orientedTile)) tileParent[orientedTile] = orientedParent;
                }

                return cells; 
            }

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
                Vector3Int orientedTile = new Vector3Int(tile.x, tile.y, direction);
                Vector3Int orientedParent = new Vector3Int(position.x, position.y, orientation);

                if (troopAtPosition.TryGetValue(tile, out Troop enc) && enc.ControllingPlayer == activePlayer) continue;
                if (!acm.Contains(tile))
                {
                    acm.Add(tile);
                    addOrientation[tile] = orientedTile;
                }
                if (!tileParent.ContainsKey(orientedTile)) tileParent[orientedTile] = orientedParent;
                if (!enc) q.Enqueue(() => GetReachableTiles(tile, direction, movePoints - 1, acm, q));
            }
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

            UIManager.UpdateScoreDisplay();
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
    }
}
