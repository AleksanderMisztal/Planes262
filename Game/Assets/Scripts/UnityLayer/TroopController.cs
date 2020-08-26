using System.Collections.Generic;
using UnityEngine;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class TroopController : MonoBehaviour
    {
        private static TroopController instance;

        [SerializeField] private GdTroop redTroopPrefab;
        [SerializeField] private GdTroop blueTroopPrefab;

        private static Dictionary<VectorTwo, GdTroop> map = new Dictionary<VectorTwo, GdTroop>();

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(this);
        }

        public static void ResetForNewGame()
        {
            foreach (GdTroop troop in map.Values) Destroy(troop.gameObject);
            map = new Dictionary<VectorTwo, GdTroop>();
        }

        public static void BeginNextRound(IEnumerable<Troop> troops)
        {
            foreach (Troop troop in troops)
            {
                GdTroop troopPrefab = troop.Player == PlayerSide.Red ? instance.redTroopPrefab : instance.blueTroopPrefab;
                GdTroop gdTroop = Instantiate(troopPrefab);
                gdTroop.Initialize(troop.Position, troop.Orientation, troop.Health);
                map.Add(troop.Position, gdTroop);
            }
        }

        public static void MoveTroop(VectorTwo position, int direction, IEnumerable<BattleResult> battleResults)
        {
            GdTroop troop = map[position];
            troop.AdjustOrientation(direction);
            ConductBattles(battleResults, troop);
            FinalizeMove(position, troop);
        }

        private static void ConductBattles(IEnumerable<BattleResult> battleResults, GdTroop troop)
        {
            foreach (BattleResult result in battleResults)
            {
                GdTroop encounter = map[troop.CellInFront];
                troop.MoveForward();
                if (result.AttackerDamaged) troop.ApplyDamage();
                if (result.DefenderDamaged) DamageDefender(encounter);
            }
        }

        private static void DamageDefender(GdTroop encounter)
        {
            encounter.ApplyDamage();
            if (encounter.Destroyed)
                map.Remove(encounter.Position);
        }

        private static void FinalizeMove(VectorTwo position, GdTroop troop)
        {
            map.Remove(position);
            if (troop.Destroyed) return;
            troop.MoveForward();
            map.Add(troop.Position, troop);
        }
    }
}
