using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal class AlwaysDamageBattles : IBattleResolver
    {
        public BattleResult GetFightResult(ITroop defender, VectorTwo attackerPosition)
        {
            return new BattleResult(true, true);
        }

        public BattleResult GetCollisionResult()
        {
            return new BattleResult(true, true);
        }
    }
}
