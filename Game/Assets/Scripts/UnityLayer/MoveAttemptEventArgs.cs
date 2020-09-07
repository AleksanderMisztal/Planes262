using Planes262.GameLogic.Utils;

namespace Planes262.UnityLayer
{
    public class MoveAttemptEventArgs
    {
        public readonly VectorTwo Position;
        public readonly int Direction;

        public MoveAttemptEventArgs(VectorTwo position, int direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}