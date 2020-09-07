using Planes262.GameLogic.Data;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic.Troops
{
    public abstract class TroopDecorator : ITroop
    {
        protected TroopDecorator(ITroop troop)
        {
            this.troop = troop;
        }

        private readonly ITroop troop;

        public PlayerSide Player => troop.Player;
        public int MovePoints => troop.MovePoints;
        public VectorTwo Position => troop.Position;
        public int Orientation => troop.Orientation;
        public bool Destroyed => troop.Destroyed;

        public int Health => troop.Health;
        
        public virtual void MoveInDirection(int direction)
        {
            troop.MoveInDirection(direction);
        }

        public virtual void FlyOverOtherTroop()
        {
            troop.FlyOverOtherTroop();
        }

        public virtual void ApplyDamage()
        {
            troop.ApplyDamage();
        }

        public virtual void ResetMovePoints()
        {
            troop.ResetMovePoints();
        }

        public virtual void CleanUpSelf()
        {
            troop.CleanUpSelf();
        }
    }
}