using System;
using GameDataStructures.Positioning;

namespace GameDataStructures.Battles
{
    [Serializable]
    public class FlakDamage
    {
        public VectorTwo flakPosition;
        public bool damaged;

        public FlakDamage(VectorTwo flakPosition, bool damaged)
        {
            this.flakPosition = flakPosition;
            this.damaged = damaged;
        }
    }
}