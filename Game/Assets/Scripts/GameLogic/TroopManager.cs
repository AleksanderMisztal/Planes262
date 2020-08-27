using System.Collections.Generic;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic
{
    public class TroopManager
    {
        private readonly Score score = new Score();
        private PlayerSide activePlayer = PlayerSide.Red;

        private readonly TroopMap troopMap;
        private readonly MoveValidator validator;
        private readonly PathFinder pathFinder;

        public TroopManager(Board board)
        {
            troopMap = new TroopMap();
            validator = new MoveValidator(troopMap, board, activePlayer);
            pathFinder = new PathFinder(troopMap);
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            troopMap.SpawnWave(troops); // HERE
            ChangeActivePlayer();
        }

        private void ChangeActivePlayer()
        {
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (Troop troop in beginningTroops)
                troop.ResetMovePoints(); // HERE

            activePlayer = activePlayer.Opponent();
            validator.ToggleActivePlayer();
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            int battleId = 0;
            Troop troop = troopMap.Get(position);
            troop.MoveInDirection(direction); // HERE

            Troop encounter = troopMap.Get(troop.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(troop);
                return;
            }
            BattleResult result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop);
            if (result.DefenderDamaged) ApplyDamage(encounter);

            troop.FlyOverOtherTroop(); // HERE
            
            while ((encounter = troopMap.Get(troop.Position)) != null && troop.Health > 0)
            {
                result = battleResults[battleId++];
                if (result.AttackerDamaged) ApplyDamage(troop);
                if (result.DefenderDamaged) ApplyDamage(encounter);

                troop.FlyOverOtherTroop(); // HERE
            }

            if (troop.Health > 0)
                troopMap.AdjustPosition(troop);
        }

        private void ApplyDamage(Troop troop)
        {
            PlayerSide opponent = troop.Player.Opponent();
            score.Increment(opponent);

            troop.ApplyDamage(); // HERE
            if (troop.Health <= 0)
                DestroyTroop(troop); // HERE
        }

        private void DestroyTroop(Troop troop)
        {
            troopMap.Remove(troop);
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
