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
        [SerializeField] private SpriteHolder flakPrefab;

        private Transform troopParent;

        private void Start()
        {
            troopParent = new GameObject("Troop Parent").transform;
        }

        public ITroop InstantiateTroop(Troop troop)
        {
            SpriteHolder troopPrefab = troop.Type == TroopType.Flak ? flakPrefab : 
                troop.Player == PlayerSide.Red ? redTroopPrefab : blueTroopPrefab;
            SpriteHolder spriteHolder = Instantiate(troopPrefab, troopParent);
            return new UnityTroopDecorator(spriteHolder, troop);
        }
    }
}