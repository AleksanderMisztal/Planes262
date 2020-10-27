using GameDataStructures;
using GameJudge.Troops;
using Planes262.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class TroopInstantiator : MonoBehaviour
    {
        [SerializeField] private SpriteHolder redTroopPrefab;
        [SerializeField] private SpriteHolder blueTroopPrefab;


        public ITroop InstantiateTroop(Troop troop)
        {
            SpriteHolder troopPrefab = troop.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            SpriteHolder spriteHolder = Instantiate(troopPrefab);
            return new UnityTroopDecorator(spriteHolder, troop);
        }
    }
}