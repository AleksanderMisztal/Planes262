using System;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class TroopDto
    {
        public string name;
        public TroopType type;
        public PlayerSide side;
        public V2Dto position;
        public int orientation;
        public int movePoints;
        public int health;
    }
}