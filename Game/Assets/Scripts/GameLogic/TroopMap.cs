using System.Collections.Generic;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Troops;

namespace Planes262.GameLogic
{
    public class TroopMap : TroopMapBase
    {
        public IEnumerable<ITroop> Troops => map.Values;

        public void ResetForNewGame()
        {
            map = new Dictionary<VectorTwo, ITroop>();
            redTroops = new HashSet<ITroop>();
            blueTroops = new HashSet<ITroop>();
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
