using System.Collections.Generic;
using System.Linq;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class UnityTroopManager : TroopManager
    {
        private readonly TroopInstantiator troopInstantiator;
        private PathFinder pathFinder;

        public UnityTroopManager(TroopInstantiator troopInstantiator)
        {
            this.troopInstantiator = troopInstantiator;
        }

        public override void ResetForNewGame(Board board)
        {
            base.ResetForNewGame(board);
            pathFinder = new PathFinder(TroopMap);
        }

        public override void BeginNextRound(IEnumerable<Troop> troops)
        {
            IEnumerable<UnityTroop> uTroops = troops.Select(t => troopInstantiator.InstantiateTroop(t));
            base.BeginNextRound(uTroops);
        }

        public HashSet<VectorTwo> GetReachableCells(VectorTwo position)
        {
            return pathFinder.GetReachableCells(position);
        }

        public List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            return pathFinder.GetDirections(start, end);
        }

        public TroopDto GetTroopDto(VectorTwo position)
        {
            Troop troop = TroopMap.Get(position);
            return troop is null ? null : new TroopDto(troop.Player, troop.Orientation);
        }
    }
}
