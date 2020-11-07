using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public interface ITroop
    {
        TroopType Type { get; }
        PlayerSide Player { get; }
        int MovePoints { get; }
        VectorTwo Position { get; }
        int Orientation { get; }
        int Health { get; }
        bool Destroyed { get; }
        IEnumerable<VectorTwo> ControlZone { get; }
        void MoveInDirection(int direction);
        void FlyOverOtherTroop();
        void ApplyDamage();
        void ResetMovePoints();
        void CleanUpSelf();
    }
}