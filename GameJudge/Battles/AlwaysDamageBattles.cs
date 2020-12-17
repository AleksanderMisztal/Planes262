using GameDataStructures;
using GameDataStructures.Battles;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal class AlwaysDamageBattles : IBattleResolver
    {
        public FightResult GetFightResult(Troop defender, VectorTwo attackerPosition, PlayerSide attackerSide) => FightResult.collision;
        public FightResult GetCollisionResult() => FightResult.collision;
        public FlakDamage GetFlakDamage(VectorTwo flakPosition) => new FlakDamage(flakPosition, true);
    }
}
