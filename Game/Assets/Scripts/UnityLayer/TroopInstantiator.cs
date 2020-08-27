using Planes262.GameLogic;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class TroopInstantiator : MonoBehaviour
    {
        [SerializeField] private TroopGO redTroopPrefab;
        [SerializeField] private TroopGO blueTroopPrefab;


        public UnityTroop InstantiateTroop(Troop troop)
        {
            TroopGO troopPrefab = troop.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            TroopGO troopGO = Instantiate(troopPrefab);
            UnityTroop uTroop = new UnityTroop(troop, troopGO);
            return uTroop;
        }
    }
}