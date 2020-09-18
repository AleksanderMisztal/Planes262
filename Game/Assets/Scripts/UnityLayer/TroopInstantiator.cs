using GameDataStructures;
using Planes262.UnityLayer.Utils;
using UnityEngine;
using ITroop = Planes262.GameLogic.Troops.ITroop;

namespace Planes262.UnityLayer
{
    public class TroopInstantiator : MonoBehaviour
    {
        [SerializeField] private SpriteHolder redTroopPrefab;
        [SerializeField] private SpriteHolder blueTroopPrefab;


        public UnityTroopDecorator InstantiateTroop(ITroop troop)
        {
            SpriteHolder troopPrefab = troop.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            SpriteHolder spriteHolder = Instantiate(troopPrefab);
            return new UnityTroopDecorator(spriteHolder, troop);
        }
    }
}