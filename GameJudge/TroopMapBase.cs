using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge
{
    public abstract class TroopMapBase
    {
        protected readonly Dictionary<VectorTwo, Troop> map = new Dictionary<VectorTwo, Troop>();

        protected readonly HashSet<Troop> redTroops = new HashSet<Troop>();
        protected readonly HashSet<Troop> blueTroops = new HashSet<Troop>();

        public void AdjustPosition(Troop troop, VectorTwo startingPosition)
        {
            map.Remove(startingPosition);
            map.Add(troop.Position, troop);
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

        public void Remove(Troop troop, VectorTwo startingPosition)
        {
            map.Remove(startingPosition);
            GetTroops(troop.Player).Remove(troop);
        }
    }
}