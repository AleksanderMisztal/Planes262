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
            bool defenderDamaged = random.Next(0, 6) < 3;
            bool attackerDamaged = defender.ControlZone.Contains(attackerPosition) && random.Next(0, 6) < 3;

            return new BattleResult(defenderDamaged, attackerDamaged);
        }

        public BattleResult GetCollisionResult()
        {
            if (random.Next(0, 6) + random.Next(0, 6) != 10) return new BattleResult();
            return new BattleResult(true, true);
        }
    }
}
