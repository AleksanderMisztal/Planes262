using System.Collections.Generic;
using Planes262.GameLogic.Utils;
using Planes262.UnityLayer;

namespace Planes262.GameLogic
{
    public class GameState
    {
        public static GameState instance = null;

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
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (var troop in beginningTroops)
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


        //Getters
        public static HashSet<VectorTwo> GetReachableCells(VectorTwo position)
        {
            return instance.pathFinder.GetReachableCells(position);
        }

        public static List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            return instance.pathFinder.GetDirections(start, end);
        }

        public static TroopDto GetTroopDto(VectorTwo position)
        {
            Troop troop = instance.troopMap.Get(position);
            if (troop is null) return null;
            return new TroopDto(troop.Player, troop.Orientation);
        }
    }
}
