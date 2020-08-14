using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Scripts.Networking;
using Scripts.UnityStuff;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.GameLogic
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance;

        public static PlayerId Side { get; set; }

        [SerializeField]
        private Troop blueTroopPrefab;
        [SerializeField]
        private Troop redTroopPrefab;

        public static Board Board { get; set; }

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

        public static bool AcceptsCalls { get; private set; } = false;

        public static int BlueScore => instance.blueScore;
        public static int RedScore => instance.redScore;

        private static Queue<Action> q = new Queue<Action>();


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

        private async void Update()
        {
            while(q.Count > 0)
            {
                await UniTask.WaitUntil(() => AcceptsCalls);
                if (q.Count <= 0 || !AcceptsCalls) return;
                q.Dequeue()();
            }
        }


        // Public interface
        public static async UniTask StartGame(PlayerId side, string oponentName, Board board)
        {
            await UIManager.StartTransitionIntoGame(side, oponentName, board);
            AcceptsCalls = true;
            await UIManager.EndTransitionIntoGame();
        }

        public static void OnCellClicked(Vector2Int cell)
        {
            if (!AcceptsCalls)
            {
                Debug.Log("Not accepting calls");
                return;
            }
            if (instance.activeTroop && instance.activeTiles.Contains(cell))
            {
                if (instance.path == null || cell != instance.target)
                {
                    instance.HighlightPathTo(cell);
                }
                else
                {
                    instance.MoveActiveTroop();
                }
                return;
            }

            instance.troopAtPosition.TryGetValue(cell, out Troop clickedTroop);

            if (clickedTroop 
                && clickedTroop.ControllingPlayer == instance.activePlayer
                && clickedTroop.ControllingPlayer == Side
                && clickedTroop.MovePoints > 0)
            {
                instance.SetActiveTroop(clickedTroop);
                return;
            }
            instance.activeTroop?.Deactivate();
            instance.activeTroop = null;
            instance.path = null;
        }

        public static void OnTroopMoved(Vector2Int position, int direction, List<BattleResult> battleResults)
        {
            q.Enqueue(async () =>
            {
                AcceptsCalls = false;

                Troop troop = instance.troopAtPosition[position];

                await troop.MoveInDirection(direction);

                foreach (var battleResult in battleResults)
                {
                    Troop encounter = instance.troopAtPosition[troop.Position];
                    instance.ApplyDamage(battleResult, troop, encounter);
                    await troop.JumpForward();
                }

                if (troop.Health > 0)
                {
                    instance.ChangeTroopPosition(troop);
                    if (troop.MovePoints <= 0 && instance.activeTroop)
                    {
                        troop.Deactivate();
                        instance.activeTroop = null;
                    }
                }
                instance.SetActiveTiles();

                AcceptsCalls = true;
            });
        }

        public static void StartNextRound(IEnumerable<SpawnTemplate> spawns)
        {
            q.Enqueue(() =>
            {
                if (instance.activeTroop)
                {
                    instance.activeTroop.Deactivate();
                    instance.activeTroop = null;
                }

                instance.SpawnNextWave(spawns);

                if (instance.activePlayer == PlayerId.Blue)
                {
                    foreach (Troop troop in instance.redTroops)
                    {
                        troop.OnTurnBegin();
                    }
                    foreach (Troop troop in instance.blueTroops)
                    {
                        troop.OnTurnEnd();
                    }
                    instance.activePlayer = PlayerId.Red;
                }
                else
                {
                    foreach (Troop troop in instance.blueTroops)
                    {
                        troop.OnTurnBegin();
                    }
                    foreach (Troop troop in instance.redTroops)
                    {
                        troop.OnTurnEnd();
                    }
                    instance.activePlayer = PlayerId.Blue;
                }
            });
        }

        public static void EndGame()
        {
            q.Enqueue(() =>
            {
                AcceptsCalls = false;
                Debug.Log("Ending the game");
                foreach (Troop troop in instance.troopAtPosition.Values)
                {
                    Destroy(troop.gameObject);
                }
                instance.troopAtPosition.Clear();
                instance.blueTroops.Clear();
                instance.redTroops.Clear();
                instance.activeTroop = null;
                instance.path = null;
                instance.blueScore = 0;
                instance.redScore = 0;
                // TODO: server should send starting player
                instance.activePlayer = PlayerId.Red;
            });
        }


        // Private functions
        private bool BlockedByAllies(Troop troop)
        {
            foreach (var cell in Hex.GetControllZone(troop.Position, troop.Orientation))
            {
                if (Board.IsInside(cell)
                    && (!troopAtPosition.TryGetValue(cell, out Troop enc) 
                        || enc.ControllingPlayer != troop.ControllingPlayer))
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
                ClientSend.MoveTroop(coords.Position, dir);
                coords.Move(dir);
            }
            path = null;
        }

        private void SetActiveTroop(Troop troop)
        {
            if (activeTroop)
            {
                activeTroop.Deactivate();
                path = null;
            }
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
                .All(c => !Board.IsInside(c) 
                            || (troopAtPosition.TryGetValue(c, out Troop enc) 
                                && enc.ControllingPlayer == activePlayer));

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

                if (!Board.IsInside(tile) 
                    || (troopAtPosition.TryGetValue(tile, out Troop enc) 
                        && enc.ControllingPlayer == activePlayer)) 
                    continue;

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

            UIManager.UpdateScoreDisplay(redScore, blueScore);
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
