using System;
using GameDataStructures.Positioning;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class TroopDto
    {
        public string name;
        public TroopType type;
        public PlayerSide side;
        public VectorTwo position;
        public int orientation;
        public int movePoints;
        public int health;
    }
}