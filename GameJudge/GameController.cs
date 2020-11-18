using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;
using GameJudge.Battles;
using GameJudge.GameEvents;
using GameJudge.Troops;
using GameJudge.Waves;

namespace GameJudge
{
    public class GameController
    {
        private PlayerSide activePlayer = PlayerSide.Blue;
        private int roundNumber = 1;
        private int movePointsLeft;

        private readonly Score score = new Score();

        private readonly IBattleResolver battleResolver;
        private readonly WaveProvider waveProvider;
        private readonly TroopMap troopMap;
        private readonly MoveValidator validator;
        private readonly TroopAi troopAi;
        
        
        public GameController(WaveProvider waveProvider, Board board) : this(new StandardBattles(), board, waveProvider) { }

        internal GameController(IBattleResolver battleResolver, Board board, WaveProvider waveProvider)
        {
            this.battleResolver = battleResolver;
            this.waveProvider = waveProvider;
            troopMap = new TroopMap(board);
            validator = new MoveValidator(troopMap, board, activePlayer);
            troopAi = new TroopAi(troopMap, board);

            IEnumerable<TroopDto> troops = waveProvider.initialTroops;
            troopMap.SpawnWave(troops);
            ResetBeginningTroops();
        }


        public event Action<TroopDto[]> TroopsSpawned;
        public event Action<TroopMovedEventArgs> TroopMoved;
        public event Action<GameEndedEventArgs> GameEnded;

        public void EndRound(PlayerSide player)
        {
            if (player == activePlayer && movePointsLeft == 0) 
                BeginRound();
            else MyLogger.Log("Move points left: " + movePointsLeft);
        }
        
        private void BeginRound()
        {
            IncrementRoundAndTogglePlayer();
            AddSpawnsForCurrentRound();
            ResetBeginningTroops();
            ExecuteAiMoves();
        }

        private void IncrementRoundAndTogglePlayer()
        {
            roundNumber++;
            activePlayer = activePlayer.Opponent();
            validator.ToggleActivePlayer();
        }

        private void AddSpawnsForCurrentRound()
        {
            IEnumerable<TroopDto> troops = waveProvider.GetTroops(roundNumber);
            TroopDto[] wave = troopMap.SpawnWave(troops);
            TroopsSpawned?.Invoke(wave);
        }

        private void ResetBeginningTroops()
        {
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer);
            foreach (Troop troop in beginningTroops) troop.ResetMovePoints();
            movePointsLeft = beginningTroops.Where(t => !t.IsFlak).Aggregate(0, (acc, t) => acc + t.MovePoints);
        }


        public void ProcessMove(PlayerSide player, VectorTwo position, int direction)
        {
            if (!validator.IsLegalMove(player, position, direction)) return;
            Troop troop = troopMap.Get(position);
            if (troop.IsFlak) MoveFlak((Flak) troop, direction);
            else
            {
                Fighter fighter = (Fighter) troop;
                MoveFighter(fighter, direction);
                if (troopAi.ShouldControl(fighter)) ControlWithAi(fighter);
            }

            CheckGameEnded();
        }

        private void CheckGameEnded()
        {
            bool redLost = troopMap.GetTroops(PlayerSide.Red).Count == 0;
            bool blueLost = troopMap.GetTroops(PlayerSide.Blue).Count == 0;
            if (redLost || blueLost)
                GameEnded?.Invoke(new GameEndedEventArgs(score));
        }

        private void MoveFlak(Flak flak, int direction)
        {
            VectorTwo startingPosition = flak.Position;
            VectorTwo targetPosition = Hex.GetAdjacentHex(flak.Position, flak.Orientation + direction);
            if (troopMap.Get(targetPosition) != null) return;
            flak.MoveInDirection(direction);
            troopMap.AdjustPosition(flak, startingPosition);
            TroopMoved?.Invoke(new TroopMovedEventArgs(startingPosition, direction, new List<BattleResult>(), score));
        }
        
        private void MoveFighter(Fighter fighter, int direction)
        {
            movePointsLeft--;
            VectorTwo startingPosition = fighter.Position;
            fighter.MoveInDirection(direction);
            foreach (Flak flak in troopMap.GetFlaks(fighter.Player.Opponent()))
            {
                if (flak.ControlZone.Contains(fighter.Position))
                {
                    MyLogger.Log($"Entered flak control zone. Position: {fighter.Position}, flak position: {flak.Position}");
                }
            }

            List<BattleResult> battleResults = new List<BattleResult>();
            Troop encounter = troopMap.Get(fighter.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(fighter, startingPosition);
                TroopMoved?.Invoke(new TroopMovedEventArgs(startingPosition, direction, battleResults, score));
                return;
            }

            BattleResult result = BattleResult.friendlyCollision;
            if (encounter.Player != fighter.Player)
                result = battleResolver.GetFightResult(encounter, startingPosition);

            battleResults.Add(result);
            if (result.AttackerDamaged) ApplyDamage(fighter, startingPosition);
            if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

            fighter.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(fighter.Position)) != null && fighter.Health > 0)
            {
                result = battleResolver.GetCollisionResult();
                battleResults.Add(result);
                if (result.AttackerDamaged) ApplyDamage(fighter, startingPosition);
                if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

                fighter.FlyOverOtherTroop();
            }

            if (fighter.Health > 0)
                troopMap.AdjustPosition(fighter, startingPosition);

            TroopMoved?.Invoke(new TroopMovedEventArgs(startingPosition, direction, battleResults, score));
        }

        private void ApplyDamage(Troop troop, VectorTwo startingPosition)
        {
            PlayerSide opponent = troop.Player.Opponent();
            score.Increment(opponent);

            if (troop.Player == activePlayer && troop.MovePoints > 0 && !troop.IsFlak)
                movePointsLeft--;

            troop.ApplyDamage();
            if (troop.Health <= 0)
                DestroyTroop(troop, startingPosition);
        }

        private void DestroyTroop(Troop troop, VectorTwo startingPosition)
        {
            troopMap.Remove(troop, startingPosition);
            if (troop.Player == activePlayer && !troop.IsFlak)
                movePointsLeft -= troop.MovePoints;
        }

        private void ExecuteAiMoves()
        {
            foreach (Troop troop in troopMap.GetTroops(activePlayer))
            {
                if (troop.IsFlak || !troopAi.ShouldControl(troop)) continue;
                ControlWithAi((Fighter) troop);
            }
        }

        private void ControlWithAi(Fighter troop)
        {
            MyLogger.Log("Will control troop");
            if (troop.IsFlak || troop.Health <= 0) return;
            while (troopAi.ShouldControl(troop) && troop.MovePoints > 0)
            {
                int direction = troopAi.GetOptimalDirection(troop);
                MoveFighter(troop, direction);
            }
        }
    }
}
