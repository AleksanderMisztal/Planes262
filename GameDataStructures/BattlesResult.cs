using GameDataStructures.Packets;

namespace GameDataStructures
{
    public class BattleResult : IWriteable, IReadable
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
        
        public static BattleResult FriendlyCollision => new BattleResult(true, true);

        public override string ToString()
        {
            return $"a: {AttackerDamaged} b: {DefenderDamaged}";
        }

        public IReadable Read(string s)
        {
            string[] props = s.Split(',');
            
            AttackerDamaged = bool.Parse(props[0]);
            DefenderDamaged = bool.Parse(props[1]);

            return this;
        }

        public string Data => $"{AttackerDamaged},{DefenderDamaged}";
    }
}
