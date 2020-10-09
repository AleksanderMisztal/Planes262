using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public abstract class TroopDecorator : ITroop
    {
        private readonly ITroop troop;
        
        protected TroopDecorator(ITroop troop) => this.troop = troop;

        public PlayerSide Player => troop.Player;
        public int MovePoints => troop.MovePoints;
        public VectorTwo Position => troop.Position;
        public int Orientation => troop.Orientation;
        public int Health => troop.Health;
        public IEnumerable<VectorTwo> ControlZone => troop.ControlZone;
        public bool Destroyed => troop.Destroyed;
        
        public virtual void MoveInDirection(int direction) => troop.MoveInDirection(direction);
        public virtual void FlyOverOtherTroop() => troop.FlyOverOtherTroop();
        public virtual void ApplyDamage() => troop.ApplyDamage();
        public virtual void ResetMovePoints() => troop.ResetMovePoints();
        public virtual void CleanUpSelf() => troop.CleanUpSelf();
    }
}