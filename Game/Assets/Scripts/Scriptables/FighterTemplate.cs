using GameDataStructures;
using UnityEngine;

namespace Planes262.Scriptables
{
    [CreateAssetMenu(fileName="New Fighter", menuName="Troops/Fighter")]
    public class FighterTemplate : TroopTemplate
    {
        public override TroopType Type => TroopType.Fighter;
    }
}
