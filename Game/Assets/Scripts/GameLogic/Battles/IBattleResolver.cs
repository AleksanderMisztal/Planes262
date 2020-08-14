namespace GameServer.GameLogic
{
    public interface IBattleResolver
    {
        BattleResult GetFightResult(Troop attacker, Troop defender);

        BattleResult GetCollisionResult();
    }
}
