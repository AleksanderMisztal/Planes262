using Planes262.GameLogic;
using Planes262.UnityLayer.Troops;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class TroopInstantiator : MonoBehaviour
    {
        [SerializeField] private SpriteHolder redTroopPrefab;
        [SerializeField] private SpriteHolder blueTroopPrefab;


        public UnityTroop InstantiateTroop(Troop troop)
        {
            SpriteHolder troopPrefab = troop.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            SpriteHolder spriteHolder = Instantiate(troopPrefab);
            UnityTroop uTroop = new UnityTroop(troop, spriteHolder);
            return uTroop;
        }
    }
}