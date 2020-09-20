using System.Collections.Generic;
using System.Linq;
using GameDataStructures;

namespace Planes262.GameLogic.Troops
{
    public static class TroopListExtension
    {
        public static  IEnumerable<Troop> ToUTroop(this IEnumerable<TroopDto> troops)
        {
            return troops.Select(t => new Troop(t.Player, t.InitialMovePoints, t.Position, t.Orientation, t.Health));
        }
    }
}