using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameJudge.Troops;

namespace GameJudge.GameEvents
{
    public class TroopsSpawnedEventArgs : EventArgs
    {
        public readonly ICollection<Troop> Troops;

        internal TroopsSpawnedEventArgs(IEnumerable<Troop> troops)
        {
            Troops = troops.ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("New round event\n");
            foreach (Troop t in Troops) sb.Append(t).Append("\n");
            return sb.ToString();
        }
    }
}
