using System;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Battles;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal class StandardBattleResolver : IBattleResolver
    {
        private static readonly Random random = new Random();

        public FightResult GetFightResult(Troop defender, VectorTwo attackerPosition, PlayerSide attackerSide)
        {
            MyLogger.Log("Resolving");
            if (defender.Player == attackerSide) return FightResult.collision;
            if (defender.Type == TroopType.Flak)
                return new FightResult(random.Next(0, 6) < 5, false);
            
            bool defenderDamaged = random.Next(0, 6) < 3;
            bool attackerInCz = defender.ControlZone.Contains(attackerPosition);
            MyLogger.Log("in cz: " + attackerInCz);
            bool attackerDamaged = attackerInCz && random.Next(0, 6) < 3;

            return new FightResult(attackerDamaged, defenderDamaged);
        }

        public FlakDamage GetFlakDamage(VectorTwo flakPosition)
            => new FlakDamage(flakPosition, random.Next(0, 6) < 3);

        public FightResult GetCollisionResult() =>
            random.Next(0, 6) + random.Next(0, 6) != 10
                ? FightResult.miss : FightResult.collision;
    }
}
