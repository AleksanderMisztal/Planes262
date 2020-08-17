using System.Collections.Generic;
using UnityEngine;
using GameServer.GameLogic;
using GameServer.Utils;

namespace Assets.Scripts.UnityStuff
{
    class TroopController : MonoBehaviour
    {
        private static TroopController instance;

        [SerializeField] private GDTroop redTroopPrefab;
        [SerializeField] private GDTroop blueTroopPrefab;

        private static Dictionary<VectorTwo, GDTroop> map;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }

        public static void ResetForNewGame()
        {
            map = new Dictionary<VectorTwo, GDTroop>();
        }

        public static void BeginNextRound(IEnumerable<Troop> troops)
        {
            foreach (var troop in troops)
            {
                var troopPrefab = troop.Player == PlayerSide.Red ? instance.redTroopPrefab : instance.blueTroopPrefab;
                GDTroop gdTroop = Instantiate(troopPrefab);
                gdTroop.Initialize(troop.Position, troop.Orientation, troop.Health);
                map.Add(troop.Position, gdTroop);
            }
        }

        public static void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            Debug.Log($"Moving troop at {position}");
            var troop = map[position];
            troop.AdjustOrientation(direction);
            foreach (var result in battleResults)
            {
                var encounter = map[troop.CellInFront];
                troop.MoveForward();
                if (result.AttackerDamaged) troop.ApplyDamage();
                if (result.DefenderDamaged) encounter.ApplyDamage();
            }
            if (!troop.Destroyed)
            {
                troop.MoveForward();
                map.Remove(position);
                map.Add(troop.Position, troop);
                Debug.Log($"Removed a troop from {position} and added at {troop.Position}");
            }
        }
    }
}
