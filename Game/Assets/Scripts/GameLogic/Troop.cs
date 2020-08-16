using GameServer.Utils;

namespace GameServer.GameLogic
{
    public class Troop
    {
        public PlayerSide Player { get; }

        public int InitialMovePoints { get; private set; }
        public int MovePoints { get; private set; }

        public VectorTwo Position { get; private set; }
        public VectorTwo StartingPosition { get; private set; }
        public int Orientation { get; private set; }

        public int Health { get; private set; }


        public Troop(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
        {
            Player = player;
            InitialMovePoints = movePoints;
            MovePoints = movePoints;
            Position = position;
            StartingPosition = position;
            Orientation = orientation;
            Health = health;
        }

        public void MoveInDirection(int direction)
        {
            if (MovePoints <= 0)
                throw new IllegalMoveException("I have no move points!");

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

        public VectorTwo[] ControllZone => Hex.GetControllZone(Position, Orientation);

        public bool InControlZone(VectorTwo position)
        {
            foreach (var cell in ControllZone)
                if (cell == position)
                    return true;

            return false;
        }

        public void ResetMovePoints()
        {
            MovePoints = InitialMovePoints;
        }

        public void ResetStartingPosition()
        {
            StartingPosition = Position;
        }
    }
}
