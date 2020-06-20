namespace Scripts.GameLogic
{
    public class BattleResult
    {
        public bool DefenderDamaged { get; set; }
        public bool AttackerDamaged { get; set; }

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
    }
}
