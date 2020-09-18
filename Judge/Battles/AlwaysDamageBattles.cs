using GameDataStructures;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal class AlwaysDamageBattles : IBattleResolver
    {
        public BattleResult GetFightResult(Troop defender, VectorTwo attackerPosition)
        {
            return new BattleResult(true, true);
        }

        public BattleResult GetCollisionResult()
        {
            return new BattleResult(true, true);
        }
    }
}
