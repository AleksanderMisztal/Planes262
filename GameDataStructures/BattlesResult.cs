using System.Collections.Generic;
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
        
        public static readonly BattleResult friendlyCollision = new BattleResult(true, true);

        public override string ToString()
        {
            return $"a: {AttackerDamaged} b: {DefenderDamaged}";
        }

        
        public IReadable Read(string s)
        {
            List<string> args = Merger.Split(s);
            
            AttackerDamaged = bool.Parse(args[0]);
            DefenderDamaged = bool.Parse(args[1]);

            return this;
        }

        public string Data => new Merger().Write(AttackerDamaged).Write(DefenderDamaged).Data;
    }
}
