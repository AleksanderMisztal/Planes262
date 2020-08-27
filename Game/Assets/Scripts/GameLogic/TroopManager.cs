using System.Collections.Generic;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic
{
    public class TroopManager
    {
        private readonly Score score = new Score();
        private PlayerSide activePlayer = PlayerSide.Red;

        private TroopMap troopMap = new TroopMap();
        private MoveValidator validator;
        private PathFinder pathFinder;

        public virtual void BeginNextRound(IEnumerable<Troop> troops)
        {
            troopMap.SpawnWave(troops);
            ChangeActivePlayer();
        }

        private void ChangeActivePlayer()
        {
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (Troop troop in beginningTroops)
                troop.ResetMovePoints();

            activePlayer = activePlayer.Opponent();
            validator.ToggleActivePlayer();
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            int battleId = 0;
            Troop troop = troopMap.Get(position);
            troop.MoveInDirection(direction);

            Troop encounter = troopMap.Get(troop.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(troop);
                return;
            }
            BattleResult result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop);
            if (result.DefenderDamaged) ApplyDamage(encounter);

            troop.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(troop.Position)) != null && troop.Health > 0)
            {
                result = battleResults[battleId++];
                if (result.AttackerDamaged) ApplyDamage(troop);
                if (result.DefenderDamaged) ApplyDamage(encounter);

                troop.FlyOverOtherTroop();
            }

            if (troop.Health > 0)
                troopMap.AdjustPosition(troop);
        }

        private void ApplyDamage(Troop troop)
        {
            PlayerSide opponent = troop.Player.Opponent();
            score.Increment(opponent);

            troop.ApplyDamage();
            if (troop.Health <= 0)
                DestroyTroop(troop);
        }

        private void DestroyTroop(Troop troop)
        {
            troopMap.Remove(troop);
        }

        public void ResetForNewGame(Board board)
        {
            foreach (Troop troop in troopMap.Troops)
            {
                troop.CleanUpSelf();
            }
            troopMap = new TroopMap();
            validator = new MoveValidator(troopMap, board, activePlayer);
            pathFinder = new PathFinder(troopMap);
        }


        //Getters
        public HashSet<VectorTwo> GetReachableCells(VectorTwo position)
        {
            return pathFinder.GetReachableCells(position);
        }

        public List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            return pathFinder.GetDirections(start, end);
        }

        public TroopDto GetTroopDto(VectorTwo position)
        {
            Troop troop = troopMap.Get(position);
            return troop is null ? null : new TroopDto(troop.Player, troop.Orientation);
        }
    }
}
