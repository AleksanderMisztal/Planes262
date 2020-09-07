namespace Planes262.GameLogic.Data
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

        public override string ToString()
        {
            return $"a: {AttackerDamaged} d: {DefenderDamaged}";
        }
    }
}
