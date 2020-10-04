using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Troop : ITroop
    {
        public PlayerSide Player { get; }

        public int InitialMovePoints { get; private set; }
        public int MovePoints { get; private set; }

        public VectorTwo Position { get; set; }
        public int Orientation { get; private set; }

        public int Health { get; private set; }
        public bool Destroyed => Health <= 0;

        public Troop(TroopDto t) : this(t.Player, t.InitialMovePoints, t.Position, t.Orientation, t.Health) { }

        public Troop(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
        {
            Player = player;
            InitialMovePoints = movePoints;
            MovePoints = movePoints;
            Position = position;
            Orientation = orientation;
            Health = health;
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
            InitialMovePoints--;
            if (MovePoints > 0)
                MovePoints--;
        }

        public IEnumerable<VectorTwo> ControlZone => Hex.GetControlZone(Position, Orientation);

        public bool InControlZone(VectorTwo position) => ControlZone.Any(cell => cell == position);

        public void ResetMovePoints()
        {
            MovePoints = InitialMovePoints;
        }

        public virtual void CleanUpSelf() { }
    }
}
