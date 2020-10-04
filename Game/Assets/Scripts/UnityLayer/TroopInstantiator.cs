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


        public ITroop InstantiateTroop(TroopDto troopDto)
        {
            SpriteHolder troopPrefab = troopDto.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            SpriteHolder spriteHolder = Instantiate(troopPrefab);
            Troop troop = new Troop(troopDto);
            return new UnityTroopDecorator(spriteHolder, troop);
        }
    }
}