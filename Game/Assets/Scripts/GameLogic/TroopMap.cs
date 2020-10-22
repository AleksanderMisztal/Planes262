using System.Collections.Generic;
using GameJudge;
using GameJudge.Troops;

namespace Planes262.GameLogic
{
    public class TroopMap : TroopMapBase
    {
        public void ResetForNewGame()
        {
            foreach (ITroop troop in map.Values) troop.CleanUpSelf();
            map.Clear();
            redTroops.Clear();
            blueTroops.Clear();
        }

        public void SpawnWave(IEnumerable<ITroop> wave)
        {
            foreach (ITroop troop in wave)
            {
                map.Add(troop.Position, troop);
                GetTroops(troop.Player).Add(troop);
            }
        }
    }
}
