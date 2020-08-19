namespace Planes262.GameLogic
{
    public class BattleResult
    {
        public bool DefenderDamaged { get; }
        public bool AttackerDamaged { get; }

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

        public override string ToString()
        {
            return $"a: {AttackerDamaged} b: {DefenderDamaged}";
        }
    }
}
