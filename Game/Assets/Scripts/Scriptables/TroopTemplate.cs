using System.Collections.Generic;
using GameDataStructures;
using UnityEngine;

namespace Planes262.Scriptables
{
    public abstract class TroopTemplate : ScriptableObject
    {
        public abstract TroopType Type { get; }

        public int movePoints;
        public int health;
        public List<Sprite> sprites;
    }
}