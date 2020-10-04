using System;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal class StandardBattles : IBattleResolver
    {
        private static readonly Random Random = new Random();

        public BattleResult GetFightResult(ITroop defender, VectorTwo attackerPosition)
        {
            bool defenderDamaged = Random.Next(0, 6) < 3;
            bool attackerDamaged = defender.ControlZone.Contains(attackerPosition) && Random.Next(0, 6) < 3;

            return new BattleResult(defenderDamaged, attackerDamaged);
        }

        public BattleResult GetCollisionResult()
        {
            if (Random.Next(0, 6) + Random.Next(0, 6) != 10) return new BattleResult();
            return new BattleResult(true, true);
        }
    }
}
