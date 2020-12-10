using System;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal class StandardBattleResolver : IBattleResolver
    {
        private static readonly Random random = new Random();

        public FightResult GetFightResult(Troop defender, VectorTwo attackerPosition, PlayerSide attackerSide)
        {
            if (defender.Player == attackerSide) return FightResult.collision;
            if (defender.Type == TroopType.Flak)
                return new FightResult(
                    random.Next(0, 6) < 5, false);
            
            bool defenderDamaged = random.Next(0, 6) < 3;
            bool attackerDamaged = defender.ControlZone.Contains(attackerPosition) && random.Next(0, 6) < 3;

            return new FightResult(defenderDamaged, attackerDamaged);
        }

        public FlakDamage GetFlakDamage(VectorTwo flakPosition)
            => new FlakDamage(flakPosition, random.Next(0, 6) < 3);

        public FightResult GetCollisionResult() =>
            random.Next(0, 6) + random.Next(0, 6) != 10
                ? FightResult.miss : FightResult.collision;
    }
}
