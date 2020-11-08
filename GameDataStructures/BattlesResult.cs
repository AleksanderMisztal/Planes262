using System;

namespace GameDataStructures
{
    [Serializable]
    public class BattleResult
    {
        public bool DefenderDamaged { get; private set; }
        public bool AttackerDamaged { get; private set; }

        public BattleResult(bool defenderDamaged, bool attackerDamaged)
        {
            DefenderDamaged = defenderDamaged;
            AttackerDamaged = attackerDamaged;
        }

        public BattleResult() 
        {
            DefenderDamaged = false;
            AttackerDamaged = false;
        }
        
        public static readonly BattleResult friendlyCollision = new BattleResult(true, true);

        public override string ToString()
        {
            return $"a: {AttackerDamaged} b: {DefenderDamaged}";
        }
    }
}
