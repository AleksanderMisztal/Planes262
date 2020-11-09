using GameDataStructures;
using GameDataStructures.Dtos;
using GameJudge.Troops;
using Planes262.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class TroopInstantiator : MonoBehaviour
    {
        [SerializeField] private SpriteHolder redTroopPrefab;
        [SerializeField] private SpriteHolder blueTroopPrefab;
        [SerializeField] private SpriteHolder flakPrefab;

        private Transform troopParent;

        private void Start()
        {
            troopParent = new GameObject("Troop Parent").transform;
        }

        public Troop InstantiateTroop(TroopDto troop)
        {
            SpriteHolder troopPrefab = troop.type == TroopType.Flak ? flakPrefab : 
                troop.side == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            SpriteHolder spriteHolder = Instantiate(troopPrefab, troopParent);
            return new UnityFighter(spriteHolder, troop);
        }
    }
}