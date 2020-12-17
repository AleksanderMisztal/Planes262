using System;

namespace GameDataStructures.Battles
{
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

        public override string ToString() => $"(a:{attackerDamaged}, d:{defenderDamaged})";
    }
}