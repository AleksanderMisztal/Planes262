using System.Collections.Generic;
using GameJudge;
using GameJudge.Troops;

namespace Planes262.GameLogic
{
    public class TroopMap : TroopMapBase
    {
        public void SpawnWave(IEnumerable<Troop> wave)
        {
            foreach (Troop troop in wave)
            {
                map.Add(troop.Position, troop);
                GetTroops(troop.Player).Add(troop);
            }
        }
    }
}
