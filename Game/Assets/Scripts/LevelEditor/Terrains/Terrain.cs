using System;
using UnityEngine;

namespace Planes262.LevelEditor.Terrains
{
    [CreateAssetMenu(fileName="New Terrain", menuName="Terrain")]
    [Serializable]
    public class Terrain : ScriptableObject
    {
        public int moveCost;
    }
}