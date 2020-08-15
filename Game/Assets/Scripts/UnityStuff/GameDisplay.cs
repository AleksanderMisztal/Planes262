using GameServer.GameLogic;
using GameServer.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UnityStuff
{
    class GameDisplay : MonoBehaviour
    {
        private static GameDisplay instance;

        [SerializeField] private GridLayout gridLayout;
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

        public static void OnCellClicked(VectorTwo cell)
        {
            
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
