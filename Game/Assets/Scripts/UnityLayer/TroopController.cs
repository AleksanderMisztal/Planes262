using System.Collections.Generic;
using UnityEngine;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class TroopController : MonoBehaviour
    {
        [SerializeField] private GdTroop redTroopPrefab;
        [SerializeField] private GdTroop blueTroopPrefab;

        private Dictionary<VectorTwo, GdTroop> map = new Dictionary<VectorTwo, GdTroop>();


        public void ResetForNewGame()
        {
            foreach (GdTroop troop in map.Values) Destroy(troop.gameObject);
            map = new Dictionary<VectorTwo, GdTroop>();
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            foreach (Troop troop in troops)
            {
                GdTroop troopPrefab = troop.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
                GdTroop gdTroop = Instantiate(troopPrefab);
                gdTroop.Initialize(troop.Position, troop.Orientation, troop.Health);
                map.Add(troop.Position, gdTroop);
            }
        }

        public void MoveTroop(VectorTwo position, int direction, IEnumerable<BattleResult> battleResults)
        {
            GdTroop troop = map[position];
            troop.AdjustOrientation(direction);
            ConductBattles(battleResults, troop);
            FinalizeMove(position, troop);
        }

        private void ConductBattles(IEnumerable<BattleResult> battleResults, GdTroop troop)
        {
            foreach (BattleResult result in battleResults)
            {
                GdTroop encounter = map[troop.CellInFront];
                troop.MoveForward();
                if (result.AttackerDamaged) troop.ApplyDamage();
                if (result.DefenderDamaged) DamageDefender(encounter);
            }
        }

        private void DamageDefender(GdTroop encounter)
        {
            encounter.ApplyDamage();
            if (encounter.Destroyed)
                map.Remove(encounter.Position);
        }

        private void FinalizeMove(VectorTwo position, GdTroop troop)
        {
            map.Remove(position);
            if (troop.Destroyed) return;
            troop.MoveForward();
            map.Add(troop.Position, troop);
        }
    }
}
