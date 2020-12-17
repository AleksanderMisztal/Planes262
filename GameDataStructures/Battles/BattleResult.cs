using System;

namespace GameDataStructures.Battles
{
    [Serializable]
    public class BattleResult
    {
        public readonly FightResult fightResult;
        public readonly FlakDamage[] flakDamages;
        
        public BattleResult(FightResult fightResult, FlakDamage[] flakDamages)
        {
            this.fightResult = fightResult;
            this.flakDamages = flakDamages;
        }
    }
}
