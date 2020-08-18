using System.Collections.Generic;
using Planes262.GameLogic.Utils;
using Planes262.UnityLayer;

namespace Planes262.GameLogic
{
    public class GameState
    {
        public static GameState Instance = null;

        private PlayerSide activePlayer = PlayerSide.Red;
        private readonly Score score = new Score();

        private readonly TroopMap troopMap;
        private readonly MoveValidator validator;
        private readonly PathFinder pathFinder;

        public GameState(Board board)
        {
            troopMap = new TroopMap();
            validator = new MoveValidator(troopMap, board, activePlayer);
            pathFinder = new PathFinder(troopMap);
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            troopMap.SpawnWave(troops);
            ChangeActivePlayer();
        }

        private void ChangeActivePlayer()
        {
            var beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (var troop in beginningTroops)
                troop.ResetMovePoints();

            activePlayer = activePlayer.Opponent();
            validator.ToggleActivePlayer();
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            var battleId = 0;
            var troop = troopMap.Get(position);
            troop.MoveInDirection(direction);

            var encounter = troopMap.Get(troop.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(troop);
                return;
            }
            var result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop);
            if (result.DefenderDamaged) ApplyDamage(encounter);

            troop.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(troop.Position)) != null && troop.Health > 0)
            {
                result = battleResults[battleId++];
                battleResults.Add(result);
                if (result.AttackerDamaged) ApplyDamage(troop);
                if (result.DefenderDamaged) ApplyDamage(encounter);

                troop.FlyOverOtherTroop();
            }

            if (troop.Health > 0)
                troopMap.AdjustPosition(troop);
        }

        private void ApplyDamage(Troop troop)
        {
            var opponent = troop.Player.Opponent();
            score.Increment(opponent);

            troop.ApplyDamage();
            if (troop.Health <= 0)
                DestroyTroop(troop);
        }

        private void DestroyTroop(Troop troop)
        {
            troopMap.Remove(troop);
        }


        //Getters
        public static HashSet<VectorTwo> GetReachableCells(VectorTwo position)
        {
            return Instance.pathFinder.GetReachableCells(position);
        }

        public static List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            return Instance.pathFinder.GetDirections(start, end);
        }

        public static TroopDto GetTroopDto(VectorTwo position)
        {
            var troop = Instance.troopMap.Get(position);
            return troop is null ? null : new TroopDto(troop.Player, troop.Orientation);
        }
    }
}
