using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataStructures;
using GameJudge.Troops;

namespace GameJudge.GameEvents
{
    public class TroopsSpawnedEventArgs : EventArgs
    {
        public readonly ICollection<TroopDto> Troops;

        internal TroopsSpawnedEventArgs(IEnumerable<Troop> troops)
        {
            Troops = troops.Select(t => new TroopDto(t.InitialMovePoints, t.Player, t.Position, t.Orientation, t.Health)).ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("New round event\n");
            foreach (TroopDto t in Troops) sb.Append(t).Append("\n");
            return sb.ToString();
        }
    }
}
