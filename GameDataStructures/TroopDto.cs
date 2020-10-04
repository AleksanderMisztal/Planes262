using GameDataStructures.Positioning;

namespace GameDataStructures
{
    public struct TroopDto
    {
        public TroopDto(PlayerSide player, int initialMovePoints, VectorTwo position, int orientation, int health)
        {
            InitialMovePoints = initialMovePoints;
            Player = player;
            Position = position;
            Orientation = orientation;
            Health = health;
        }

        public PlayerSide Player { get; }
        public int InitialMovePoints { get; }
        public VectorTwo Position { get; private set; }
        public int Orientation { get; }
        public int Health { get; }

        public void AdjustPosition(VectorTwo newPosition)
        {
            Position = newPosition;
        }
    }
}
