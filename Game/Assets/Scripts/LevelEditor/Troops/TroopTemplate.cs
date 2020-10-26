using System.Collections.Generic;
using GameDataStructures;
using UnityEngine;

namespace Planes262.LevelEditor.Troops
{
    public abstract class TroopTemplate : ScriptableObject
    {
        public abstract TroopType Type { get; }

        public int movePoints;
        public int health;
        public List<Sprite> sprites;

        public Sprite Sprite => sprites[health - 1];
    }
}