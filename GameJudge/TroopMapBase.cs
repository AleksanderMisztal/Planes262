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
        
        private readonly HashSet<Flak> redFlaks = new HashSet<Flak>();
        private readonly HashSet<Flak> blueFlaks = new HashSet<Flak>();

        public void AdjustPosition(Troop troop, VectorTwo startingPosition)
        {
            map.Remove(startingPosition);
            map.Add(troop.Position, troop);
        }

        public HashSet<Flak> GetFlaks(PlayerSide player) => player == PlayerSide.Red ? redFlaks : blueFlaks;

        public HashSet<Troop> GetTroops(PlayerSide player) => player == PlayerSide.Red ? redTroops : blueTroops;

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