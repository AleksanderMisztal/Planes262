using System;
using GameDataStructures.Positioning;

namespace GameDataStructures
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

    [Serializable]
    public class FightResult
    {
        public readonly bool attackerDamaged;
        public readonly bool defenderDamaged;

        public FightResult(bool attackerDamaged, bool defenderDamaged)
        {
            this.attackerDamaged = attackerDamaged;
            this.defenderDamaged = defenderDamaged;
        }

        public static FightResult miss = new FightResult(false, false);
        public static FightResult collision = new FightResult(true, true);
    }
        
        
    [Serializable]
    public class FlakDamage
    {
        public VectorTwo flakPosition;
        public bool damaged;

        public FlakDamage(VectorTwo flakPosition, bool damaged)
        {
            this.flakPosition = flakPosition;
            this.damaged = damaged;
        }
    }
}
