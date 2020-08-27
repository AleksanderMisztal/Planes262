using System.Collections.Generic;
using UnityEngine;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class UnityTroopController
    {
        private TroopInstantiator troopInstantiator;
        
        private Dictionary<VectorTwo, UnityTroop> map = new Dictionary<VectorTwo, UnityTroop>();

        public UnityTroopController(TroopInstantiator troopInstantiator)
        {
            this.troopInstantiator = troopInstantiator;
        }


        public void ResetForNewGame()
        {
            foreach (UnityTroop troop in map.Values) Object.Destroy(troop.gameObject);
            map = new Dictionary<VectorTwo, UnityTroop>();
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            foreach (Troop troop in troops)
            {
                UnityTroop uTroop = troopInstantiator.InstantiateTroop(troop);
                map.Add(troop.Position, uTroop);
            }
        }

        public void MoveTroop(VectorTwo position, int direction, IEnumerable<BattleResult> battleResults)
        {
            UnityTroop troop = map[position];
            troop.AdjustOrientation(direction);
            ConductBattles(battleResults, troop);
            FinalizeMove(position, troop);
        }

        private void ConductBattles(IEnumerable<BattleResult> battleResults, UnityTroop troop)
        {
            foreach (BattleResult result in battleResults)
            {
                UnityTroop encounter = map[troop.CellInFront];
                troop.MoveForward();
                if (result.AttackerDamaged) troop.ApplyDamage();
                if (result.DefenderDamaged) DamageDefender(encounter);
            }
        }

        private void DamageDefender(UnityTroop encounter)
        {
            encounter.ApplyDamage();
            if (encounter.Destroyed)
                map.Remove(encounter.Position);
        }

        private void FinalizeMove(VectorTwo position, UnityTroop troop)
        {
            map.Remove(position);
            if (troop.Destroyed) return;
            troop.MoveForward();
            map.Add(troop.Position, troop);
        }
    }
}
