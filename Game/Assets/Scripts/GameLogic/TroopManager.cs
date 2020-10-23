using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace Planes262.GameLogic
{
    public class TroopManager
    {
        private readonly TroopMap troopMap;
        private PlayerSide activePlayer = PlayerSide.Red;

        public TroopManager(TroopMap troopMap)
        {
            this.troopMap = troopMap;
        }

        public void BeginNextRound(IEnumerable<ITroop> troops)
        {
            troopMap.SpawnWave(troops);
            
            activePlayer = activePlayer.Opponent();
            
            HashSet<ITroop> beginningTroops = troopMap.GetTroops(activePlayer);
            foreach (ITroop troop in beginningTroops) troop.ResetMovePoints();
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            int battleId = 0;
            ITroop troop = troopMap.Get(position);
            VectorTwo startingPosition = troop.Position;
            troop.MoveInDirection(direction);

            ITroop encounter = troopMap.Get(troop.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(troop, startingPosition);
                return;
            }
            BattleResult result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop, startingPosition);
            if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

            troop.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(troop.Position)) != null && !troop.Destroyed)
            {
                result = battleResults[battleId++];
                if (result.AttackerDamaged) ApplyDamage(troop, startingPosition);
                if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

                troop.FlyOverOtherTroop();
            }

            if (!troop.Destroyed)
                troopMap.AdjustPosition(troop, startingPosition);
        }

        private void ApplyDamage(ITroop troop, VectorTwo startingPosition)
        {
            troop.ApplyDamage();
            if (troop.Destroyed)
                troopMap.Remove(troop, startingPosition);
        }
    }
}
