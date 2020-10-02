using GameDataStructures;
using GameDataStructures.Positioning;

namespace Planes262.UnityLayer
{
    public class MoveAttemptEventArgs
    {
        public readonly PlayerSide Side;
        public readonly VectorTwo Position;
        public readonly int Direction;

        public MoveAttemptEventArgs(PlayerSide side, VectorTwo position, int direction)
        {
            Side = side;
            Position = position;
            Direction = direction;
        }
    }
}