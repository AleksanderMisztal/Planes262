using System.Collections.Generic;
using GameDataStructures;
using ITroop = Planes262.GameLogic.Troops.ITroop;

namespace Planes262.GameLogic
{
    public class TroopMap
    {
        private Dictionary<VectorTwo, ITroop> map = new Dictionary<VectorTwo, ITroop>();
        private HashSet<ITroop> redTroops = new HashSet<ITroop>();
        private HashSet<ITroop> blueTroops = new HashSet<ITroop>();
        public IEnumerable<ITroop> Troops => map.Values;


        public void ResetForNewGame()
        {
            map = new Dictionary<VectorTwo, ITroop>();
            redTroops = new HashSet<ITroop>();
            blueTroops = new HashSet<ITroop>();
        }

        public void AdjustPosition(ITroop troop, VectorTwo startingPosition)
        {
            map.Remove(startingPosition);
            map.Add(troop.Position, troop);
        }

        public HashSet<ITroop> GetTroops(PlayerSide player)
        {
            return player == PlayerSide.Red ? redTroops : blueTroops;
        }

        public ITroop Get(VectorTwo position)
        {
            try
            {
                return map[position];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public void Remove(ITroop troop, VectorTwo startingPosition)
        {
            map.Remove(startingPosition);
            GetTroops(troop.Player).Remove(troop);
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
