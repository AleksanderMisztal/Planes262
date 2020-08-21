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
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }

        public static void ResetForNewGame()
        {
            foreach (GdTroop troop in map.Values)
                Destroy(troop);
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

        public static void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            GdTroop troop = map[position];
            troop.AdjustOrientation(direction);
            ConductBattles(battleResults, troop);
            FinalizeMoveIfNotDestroyed(position, troop);
        }

        private static void ConductBattles(List<BattleResult> battleResults, GdTroop troop)
        {
            foreach (BattleResult result in battleResults)
            {
                Debug.Log($"Position is {troop.Position}, in front {troop.CellInFront}");
                GdTroop encounter = map[troop.CellInFront];
                troop.MoveForward();
                if (result.AttackerDamaged) ApplyDamage(troop);
                if (result.DefenderDamaged) ApplyDamage(encounter);
            }
        }

        private static void ApplyDamage(GdTroop troop)
        {
            troop.ApplyDamage();
            if (troop.Destroyed)
                map.Remove(troop.Position);
        }

        private static void FinalizeMoveIfNotDestroyed(VectorTwo position, GdTroop troop)
        {
            if (troop.Destroyed) return;
            troop.MoveForward();
            map.Remove(position);
            map.Add(troop.Position, troop);
        }
    }
}
