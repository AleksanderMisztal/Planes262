using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public abstract class Troop
    {
        public PlayerSide Player { get; }
        public abstract TroopType Type { get; }

        private int initialMovePoints;
        public int MovePoints { get; private set; }

        public VectorTwo Position { get; private set; }
        public int Orientation { get; private set; }

        public int Health { get; private set; }
        public bool Destroyed => Health <= 0;

        protected Troop(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
        {
            Player = player;
            initialMovePoints = movePoints;
            MovePoints = movePoints;
            Position = position;
            Orientation = orientation;
            Health = health;
        }
        
        protected Troop(TroopDto dto)
        {
            Player = dto.side;
            initialMovePoints = dto.movePoints;
            MovePoints = dto.movePoints;
            Position = dto.position;
            Orientation = dto.orientation;
            Health = dto.health;
        }

        public virtual void MoveInDirection(int direction)
        {
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

        public virtual IEnumerable<VectorTwo> ControlZone => Hex.GetControlZone(Position, Orientation);

        public bool InControlZone(VectorTwo position) => ControlZone.Any(cell => cell == position);

        public virtual void ResetMovePoints()
        {
            MovePoints = initialMovePoints;
        }
    }
}
