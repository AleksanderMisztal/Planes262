using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge
{
    public abstract class TroopMapBase
    {
        protected Dictionary<VectorTwo, ITroop> map = new Dictionary<VectorTwo, ITroop>();

        protected HashSet<ITroop> redTroops = new HashSet<ITroop>();
        protected HashSet<ITroop> blueTroops = new HashSet<ITroop>();

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
    }
}