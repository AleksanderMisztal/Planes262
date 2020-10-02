using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameJudge.Battles;
using GameJudge.GameEvents;
using GameJudge.Troops;
using GameJudge.Waves;

namespace GameJudge
{
    public class GameController
    {
        private PlayerSide activePlayer = PlayerSide.Red;
        private int roundNumber;
        private int movePointsLeft;

        private readonly Score score = new Score();

        private readonly IBattleResolver battleResolver;
        private readonly WaveProvider waveProvider;
        private readonly Board board;
        private readonly TroopMap troopMap;
        private readonly MoveValidator validator;
        private readonly TroopAi troopAi;
        
        
        public GameController(WaveProvider waveProvider, Board board) : this(new StandardBattles(), board, waveProvider) { }

        internal GameController(IBattleResolver battleResolver, Board board, WaveProvider waveProvider)
        {
            this.battleResolver = battleResolver;
            this.waveProvider = waveProvider;
            this.board = board;
            troopMap = new TroopMap(board);
            validator = new MoveValidator(troopMap, board, activePlayer);
            troopAi = new TroopAi(troopMap, board);
        }


        public event Action<TroopsSpawnedEventArgs> TroopsSpawned;
        public event Action<TroopMovedEventArgs> TroopMoved;
        public event Action<GameEndedEventArgs> GameEnded;


        public void BeginGame()
        {
            if (roundNumber != 0) throw new Exception("This game controller has already been initialized");
            BeginRound();
        }

        private void BeginRound()
        {
            roundNumber++;
            AddSpawnsForCurrentRound();
            ChangeActivePlayer();
            ExecuteAiMoves();
        }

        private void ChangeActivePlayer()
        {
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (Troop troop in beginningTroops)
                troop.ResetMovePoints();

            activePlayer = activePlayer.Opponent();
            validator.ToggleActivePlayer();
            SetInitialMovePointsLeft(activePlayer);
        }

        private void AddSpawnsForCurrentRound()
        {
            List<Troop> wave = waveProvider.GetTroops(roundNumber);
            wave = troopMap.SpawnWave(wave);
            TroopsSpawned?.Invoke(new TroopsSpawnedEventArgs(wave));
        }

        private void SetInitialMovePointsLeft(PlayerSide player)
        {
            HashSet<Troop> troops = troopMap.GetTroops(player);
            movePointsLeft = troops.Aggregate(0, (acc, t) => acc + t.InitialMovePoints);
        }


        public void ProcessMove(PlayerSide player, VectorTwo position, int direction)
        {
            if (!validator.IsLegalMove(player, position, direction)) return;
            Troop troop = troopMap.Get(position);
            MoveTroop(position, direction);
            if (!board.IsInside(troop.Position)) ControlWithAi(troop);

            while (!GameHasEnded())
            {
                if (movePointsLeft == 0)
                    BeginRound();
                else return;
            }
            GameEnded?.Invoke(new GameEndedEventArgs(score));
        }

        private bool GameHasEnded()
        {
            bool redLost = troopMap.GetTroops(PlayerSide.Red).Count == 0 && waveProvider.MaxRedWave <= roundNumber;
            bool blueLost = troopMap.GetTroops(PlayerSide.Blue).Count == 0 && waveProvider.MaxBlueWave <= roundNumber;

            return redLost || blueLost;
        }

        private void MoveTroop(VectorTwo position, int direction)
        {
            movePointsLeft--;

            Troop troop = troopMap.Get(position);
            VectorTwo startingPosition = troop.Position;
            troop.MoveInDirection(direction);

            List<BattleResult> battleResults = new List<BattleResult>();
            Troop encounter = troopMap.Get(troop.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(troop, startingPosition);
                TroopMoved?.Invoke(new TroopMovedEventArgs(position, direction, battleResults));
                return;
            }

            BattleResult result = BattleResult.FriendlyCollision;
            if (encounter.Player != troop.Player)
                result = battleResolver.GetFightResult(encounter, startingPosition);

            battleResults.Add(result);
            if (result.AttackerDamaged) ApplyDamage(troop, startingPosition);
            if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

            troop.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(troop.Position)) != null && troop.Health > 0)
            {
                result = battleResolver.GetCollisionResult();
                battleResults.Add(result);
                if (result.AttackerDamaged) ApplyDamage(troop, startingPosition);
                if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

                troop.FlyOverOtherTroop();
            }

            if (troop.Health > 0)
                troopMap.AdjustPosition(troop, startingPosition);

            TroopMoved?.Invoke(new TroopMovedEventArgs(position, direction, battleResults));
        }

        private void ApplyDamage(Troop troop, VectorTwo startingPosition)
        {
            PlayerSide opponent = troop.Player.Opponent();
            score.Increment(opponent);

            if (troop.Player == activePlayer && troop.MovePoints > 0)
                movePointsLeft--;

            troop.ApplyDamage();
            if (troop.Health <= 0)
                DestroyTroop(troop, startingPosition);
        }

        private void DestroyTroop(Troop troop, VectorTwo startingPosition)
        {
            troopMap.Remove(troop, startingPosition);
            if (troop.Player == activePlayer)
                movePointsLeft -= troop.MovePoints;
        }

        private void ExecuteAiMoves()
        {
            foreach (Troop troop in troopMap.GetTroops(activePlayer))
            {
                if (!troopAi.ShouldControl(troop)) continue;
                ControlWithAi(troop);
            }
        }

        private void ControlWithAi(Troop troop)
        {
            while (troopAi.ShouldControl(troop) && troop.MovePoints > 0)
            {
                int direction = troopAi.GetOptimalDirection(troop);
                MoveTroop(troop.Position, direction);
            }
        }
    }
}
