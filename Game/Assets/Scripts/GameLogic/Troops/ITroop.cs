using Planes262.GameLogic.Data;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic.Troops
{
    public interface ITroop
    {
        PlayerSide Player { get; }
        int MovePoints { get; }
        VectorTwo Position { get; }
        int Orientation { get; }
        int Health { get; }
        bool Destroyed { get; }
        void MoveInDirection(int direction);
        void FlyOverOtherTroop();
        void ApplyDamage();
        void ResetMovePoints();
        void CleanUpSelf();
    }
}