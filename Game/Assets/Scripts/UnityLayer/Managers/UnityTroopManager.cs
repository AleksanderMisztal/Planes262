using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameJudge.Troops;
using Planes262.GameLogic;

namespace Planes262.UnityLayer.Managers
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
