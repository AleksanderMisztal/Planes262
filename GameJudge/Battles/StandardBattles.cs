using System;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal class StandardBattles : IBattleResolver
    {
        private static readonly Random random = new Random();

        public BattleResult GetFightResult(ITroop defender, VectorTwo attackerPosition)
        {
            if (defender.Type == TroopType.Flak) return GetFlakDamage(defender, attackerPosition);
            
            bool defenderDamaged = random.Next(0, 6) < 3;
            bool attackerDamaged = defender.ControlZone.Contains(attackerPosition) && random.Next(0, 6) < 3;

            return new BattleResult(defenderDamaged, attackerDamaged);
        }

        private BattleResult GetFlakDamage(ITroop flak, VectorTwo attackerPosition)
        {
            return attackerPosition == flak.Position 
                ? new BattleResult(false, random.Next(0, 6) < 5) 
                : new BattleResult(false, random.Next(0, 6) < 3);
        }

        public BattleResult GetCollisionResult()
        {
            if (random.Next(0, 6) + random.Next(0, 6) != 10) return new BattleResult();
            return new BattleResult(true, true);
        }
    }
}
