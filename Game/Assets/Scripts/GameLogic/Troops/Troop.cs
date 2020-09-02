using System;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic.Troops
{
    public interface ITroop
    {
        PlayerSide Player { get; }
        int MovePoints { get; }
        VectorTwo Position { get; }
        int Orientation { get; }
        bool Destroyed { get; }
        void MoveInDirection(int direction);
        void FlyOverOtherTroop();
        void ApplyDamage();
        void ResetMovePoints();
        void CleanUpSelf();
    }

    public class Troop : ITroop
    {
        public PlayerSide Player { get; }

        private int initialMovePoints;
        public int MovePoints { get; private set; }

        public VectorTwo Position { get; private set; }
        public int Orientation { get; private set; }

        protected int Health;
        public bool Destroyed => Health <= 0;


        public Troop(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
        {
            Player = player;
            initialMovePoints = movePoints;
            MovePoints = movePoints;
            Position = position;
            Orientation = orientation;
            this.Health = health;
        }

        protected Troop(Troop t) : this(t.Player, t.MovePoints, t.Position, t.Orientation, t.Health) { }
        
        public virtual void MoveInDirection(int direction)
        {
            if (MovePoints <= 0)
                throw new Exception("I have no move points!");

            MovePoints--;
            Orientation = (6 + Orientation + direction) % 6;
            Position = Hex.GetAdjacentHex(Position, Orientation);
        }

        public virtual void FlyOverOtherTroop()
        {
            Position = Hex.GetAdjacentHex(Position, Orientation);
        }

        public virtual void ApplyDamage()
        {
            Health--;
            initialMovePoints--;
            if (MovePoints > 0)
                MovePoints--;
        }

        public void ResetMovePoints()
        {
            MovePoints = initialMovePoints;
        }

        public virtual void CleanUpSelf() { }
    }
}
