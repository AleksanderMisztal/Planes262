using System;
using GameDataStructures.Positioning;

namespace GameDataStructures
{
    [Serializable]
    public class TroopDto
    {
        public TroopType type;
        public PlayerSide side;
        public VectorTwo position;
        public int orientation;
        public int movePoints;
        public int health;
    }
}