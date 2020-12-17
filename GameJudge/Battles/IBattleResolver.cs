using GameDataStructures;
using GameDataStructures.Battles;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Battles
{
    internal interface IBattleResolver
    {
        FightResult GetFightResult(Troop defender, VectorTwo attackerPosition, PlayerSide attackerSide);

        FightResult GetCollisionResult();
        FlakDamage GetFlakDamage(VectorTwo flakPosition);
    }
}
