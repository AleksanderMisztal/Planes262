using System.Collections.Generic;
using GameDataStructures;
using UnityEngine;

namespace Planes262.LevelEditor.Troops
{
    [CreateAssetMenu(fileName="New Troop", menuName="Troop")]
    public class TroopTemplate : ScriptableObject
    {
        public TroopType type;

        public PlayerSide side;
        public int movePoints;
        public int health;
        public List<Sprite> sprites;

        public Sprite Sprite => sprites[health - 1];
    }
}