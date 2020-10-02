using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal interface IBattleResolver
    {
        BattleResult GetFightResult(Troop defender, VectorTwo attackerPosition);

        BattleResult GetCollisionResult();
    }
}
