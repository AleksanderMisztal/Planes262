using Planes262.GameLogic;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class TroopInstantiator : MonoBehaviour
    {
        [SerializeField] private UnityTroop redTroopPrefab;
        [SerializeField] private UnityTroop blueTroopPrefab;


        public UnityTroop InstantiateTroop(Troop troop)
        {
            UnityTroop troopPrefab = troop.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            UnityTroop unityTroop = Instantiate(troopPrefab);
            unityTroop.Initialize(troop.Position, troop.Orientation, troop.Health);
            return unityTroop;
        }
    }
}