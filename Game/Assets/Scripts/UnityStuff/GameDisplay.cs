using GameServer.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UnityStuff
{
    class GameDisplay : MonoBehaviour
    {
        private static GameDisplay instance;

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

        [SerializeField] private GridLayout gridLayout;
        [SerializeField] private GDTroop troopPrefab;

        public static void BeginNextRound(IEnumerable<Troop> troops)
        {
            foreach (var troop in troops)
            {
                GDTroop gdTroop = Instantiate(instance.troopPrefab);
                gdTroop.Initialize(troop.Position, troop.Orientation, troop.Player);
            }
        }
    }
}
