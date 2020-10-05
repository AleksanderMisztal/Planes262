using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
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

        public void AdjustPosition(VectorTwo newPosition)
        {
            Position = newPosition;
        }

        public virtual void MoveInDirection(int direction)
        {
            MovePoints--;
            Orientation = (6 + Orientation + direction) % 6;
            Position = Hex.GetAdjacentHex(Position, Orientation);
        }

        public void FlyOverOtherTroop()
        {
            Position = Hex.GetAdjacentHex(Position, Orientation);
        }

        public void ApplyDamage()
        {
            Health--;
            initialMovePoints--;
            if (MovePoints > 0)
                MovePoints--;
        }

        public IEnumerable<VectorTwo> ControlZone => Hex.GetControlZone(Position, Orientation);

        public bool InControlZone(VectorTwo position) => ControlZone.Any(cell => cell == position);

        public void ResetMovePoints()
        {
            MovePoints = initialMovePoints;
        }

        public virtual void CleanUpSelf() { }
    }
}
