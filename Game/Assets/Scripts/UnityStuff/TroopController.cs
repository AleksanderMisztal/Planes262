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

        private static Dictionary<VectorTwo, GDTroop> map = new Dictionary<VectorTwo, GDTroop>();

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

        public static void BeginNextRound(IEnumerable<Troop> troops)
        {
            foreach (var troop in troops)
            {
                var troopPrefab = troop.Player == PlayerSide.Red ? instance.redTroopPrefab : instance.blueTroopPrefab;
                GDTroop gdTroop = Instantiate(troopPrefab);
                gdTroop.Initialize(troop.Position, troop.Orientation, troop.Player);
            }
        }

        public static void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
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
            }
        }
    }
}
