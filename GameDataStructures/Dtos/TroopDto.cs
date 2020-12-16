using System;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class TroopDto
    {
        public TroopDto(string name, TroopType type, PlayerSide side, V2Dto position, int orientation, int movePoints, int health)
        {
            this.name = name;
            this.type = type;
            this.side = side;
            this.position = position;
            this.orientation = orientation;
            this.movePoints = movePoints;
            this.health = health;
        }

        public TroopDto() { }

        public string name;
        public TroopType type;
        public PlayerSide side;
        public V2Dto position;
        public int orientation;
        public int movePoints;
        public int health;
    }
}