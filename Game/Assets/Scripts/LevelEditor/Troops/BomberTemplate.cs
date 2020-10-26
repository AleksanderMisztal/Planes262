using GameDataStructures;
using UnityEngine;

namespace Planes262.LevelEditor.Troops
{
    [CreateAssetMenu(fileName="New Bomber", menuName="Troops/Bomber")]
    public class BomberTemplate : TroopTemplate
    {
        public override TroopType Type => TroopType.Bomber;
    }
}