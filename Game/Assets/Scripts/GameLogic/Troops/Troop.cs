using System;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace Planes262.GameLogic.Troops
{
    public class Troop : ITroop
    {
        public PlayerSide Player { get; }

        private int initialMovePoints;
        public int MovePoints { get; private set; }

        public VectorTwo Position { get; private set; }
        public int Orientation { get; private set; }

        public int Health { get; private set; }
        public bool Destroyed => Health <= 0;


        public Troop(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
        {
            Player = player;
            initialMovePoints = movePoints;
            MovePoints = movePoints;
            Position = position;
            Orientation = orientation;
            Health = health;
        }

        public Troop(TroopDto t) : this(t.Player, t.InitialMovePoints, t.Position, t.Orientation, t.Health) { }

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
