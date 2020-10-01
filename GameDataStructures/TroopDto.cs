namespace GameDataStructures
{
    public class TroopDto
    {
        public TroopDto(int initialMovePoints, PlayerSide player, VectorTwo position, int orientation, int health)
        {
            InitialMovePoints = initialMovePoints;
            Player = player;
            Position = position;
            Orientation = orientation;
            Health = health;
        }

        public PlayerSide Player { get; }
        public int InitialMovePoints { get; }
        public VectorTwo Position { get; }
        public int Orientation { get; }
        public int Health { get; }
    }
}
