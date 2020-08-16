using System.Collections.Generic;
using UnityEngine;
using GameServer.GameLogic;

namespace Assets.Scripts.UnityStuff
{
    class TroopInstantiator : MonoBehaviour
    {
        private static TroopInstantiator instance;

        [SerializeField] private GDTroop redTroopPrefab;
        [SerializeField] private GDTroop blueTroopPrefab;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
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
    }
}
