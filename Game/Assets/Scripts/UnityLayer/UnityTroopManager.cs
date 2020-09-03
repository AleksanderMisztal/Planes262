using System.Collections.Generic;
using System.Linq;
using Planes262.GameLogic;
using Planes262.GameLogic.Troops;

namespace Planes262.UnityLayer
{
    public class UnityTroopManager : TroopManager
    {
        private readonly TroopInstantiator troopInstantiator;

        public UnityTroopManager(TroopMap troopMap, TroopInstantiator troopInstantiator, Score score) : base(troopMap, score)
        {
            this.troopInstantiator = troopInstantiator;
        }

        public override void BeginNextRound(IEnumerable<ITroop> troops)
        {
            IEnumerable<UnityTroopDecorator> uTroops = troops.Select(t => troopInstantiator.InstantiateTroop(t));
            base.BeginNextRound(uTroops);
        }
    }
}
