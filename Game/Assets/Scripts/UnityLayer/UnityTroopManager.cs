using System.Collections.Generic;
using System.Linq;
using Planes262.GameLogic;
using Planes262.UnityLayer.Troops;

namespace Planes262.UnityLayer
{
    public class UnityTroopManager : TroopManager
    {
        private readonly TroopInstantiator troopInstantiator;

        public UnityTroopManager(TroopMap troopMap, TroopInstantiator troopInstantiator, Score score) : base(troopMap, score)
        {
            this.troopInstantiator = troopInstantiator;
        }

        public override void BeginNextRound(IEnumerable<Troop> troops)
        {
            IEnumerable<UnityTroop> uTroops = troops.Select(t => troopInstantiator.InstantiateTroop(t));
            base.BeginNextRound(uTroops);
        }
    }
}
