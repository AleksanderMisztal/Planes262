using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Messages.Server;
using GameDataStructures.Positioning;
using GameJudge.Battles;
using GameJudge.Troops;

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
        
        
        public GameController(WaveProvider waveProvider, Board board) : this(new StandardBattleResolver(), board, waveProvider) { }

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
        public event Action<TroopMovedMessage> TroopMoved;
        public event Action<GameEndedMessage> GameEnded;

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
                GameEnded?.Invoke(new GameEndedMessage(score.Info));
        }

        private void MoveFlak(Flak flak, int direction)
        {
            VectorTwo startingPosition = flak.Position;
            VectorTwo targetPosition = Hex.GetAdjacentHex(flak.Position, flak.Orientation + direction);
            if (troopMap.Get(targetPosition) != null) return;
            flak.MoveInDirection(direction);
            troopMap.AdjustPosition(flak, startingPosition);
            TroopMoved?.Invoke(new TroopMovedMessage(startingPosition, direction, null, score.Info));
        }
        
        private void MoveFighter(Fighter fighter, int direction)
        {
            List<BattleResult> battleResults = new List<BattleResult>();
            FlakDamage[] flakDamages;
            VectorTwo startingPosition = fighter.Position;

            void ApplyFlakDamages()
            {
                flakDamages = GetFlakDamages(fighter);
                foreach (FlakDamage _ in flakDamages.Where(d => d.damaged))
                {
                    ApplyDamage(fighter, startingPosition);
                    if (fighter.Destroyed)
                    {
                        battleResults.Add(new BattleResult(null, flakDamages));
                        EndMove();
                        return;
                    }
                }
            }

            void ApplyFightDamage(bool isFirst)
            {
                Troop encounter = troopMap.Get(fighter.Position);
                FightResult fightResult = isFirst
                    ? battleResolver.GetFightResult(encounter, startingPosition, fighter.Player)
                    : battleResolver.GetCollisionResult();
                battleResults.Add(new BattleResult(fightResult, flakDamages));
                if (fightResult.attackerDamaged) ApplyDamage(fighter, startingPosition);
                if (fightResult.defenderDamaged) ApplyDamage(encounter, encounter.Position);
            }

            void EndMove()
            {
                if (!fighter.Destroyed) troopMap.AdjustPosition(fighter, startingPosition);
                TroopMoved?.Invoke(new TroopMovedMessage(startingPosition, direction, battleResults.ToArray(), score.Info));
            }

            fighter.MoveInDirection(direction);
            movePointsLeft--;

            ApplyFlakDamages();
            if (fighter.Destroyed) return;

            bool EnteredEmpty() => troopMap.Get(fighter.Position) == null;
            if (EnteredEmpty())
            {
                battleResults.Add(new BattleResult(null, flakDamages));
                EndMove();
                return;
            }

            ApplyFightDamage(true);
            while (fighter.Health > 0)
            {
                fighter.FlyOverOtherTroop();
                
                ApplyFlakDamages();
                if (fighter.Destroyed) return;

                if (EnteredEmpty())
                {
                    battleResults.Add(new BattleResult(null, flakDamages));
                    break;
                }
                ApplyFightDamage(false);
            }
            EndMove();
        }

        private FlakDamage[] GetFlakDamages(Fighter fighter)
        {
            return troopMap.GetFlaks(fighter.Player.Opponent())
                .Where(f => f.ControlZone.Contains(fighter.Position))
                .Select(f => battleResolver.GetFlakDamage(f.Position))
                .ToArray();
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
