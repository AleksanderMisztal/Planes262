using System.Collections.Generic;
using Planes262.GameLogic.Troops;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic
{
    public class TroopMap
    {
        private Dictionary<VectorTwo, Troop> map = new Dictionary<VectorTwo, Troop>();
        private HashSet<Troop> redTroops = new HashSet<Troop>();
        private HashSet<Troop> blueTroops = new HashSet<Troop>();
        public IEnumerable<Troop> Troops => map.Values;


        public void ResetForNewGame()
        {
            map = new Dictionary<VectorTwo, Troop>();
            redTroops = new HashSet<Troop>();
            blueTroops = new HashSet<Troop>();
        }

        public void AdjustPosition(Troop troop)
        {
            map.Remove(troop.StartingPosition);
            map.Add(troop.Position, troop);
            troop.ResetStartingPosition();
        }

        public HashSet<Troop> GetTroops(PlayerSide player)
        {
            return player == PlayerSide.Red ? redTroops : blueTroops;
        }

        public Troop Get(VectorTwo position)
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

        public void Remove(Troop troop)
        {
            map.Remove(troop.StartingPosition);
            GetTroops(troop.Player).Remove(troop);
        }

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
