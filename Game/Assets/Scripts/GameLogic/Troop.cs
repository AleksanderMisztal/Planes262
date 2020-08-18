using Planes262.GameLogic.Utils;
using Planes262.GameLogic.Exceptions;

namespace Planes262.GameLogic
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


        // Factories
        public static Troop Red(int x, int y)
        {
            return new Troop(PlayerSide.Red, 5, new VectorTwo(x, y), 3, 2);
        }

        public static Troop Blue(int x, int y)
        {
            return new Troop(PlayerSide.Blue, 5, new VectorTwo(x, y), 0, 2);
        }

        public static Troop Blue(int x, int y, int movePoints)
        {
            return new Troop(PlayerSide.Blue, movePoints, new VectorTwo(x, y), 0, 2);
        }

        public static Troop Red(VectorTwo position)
        {
            return new Troop(PlayerSide.Red, 5, position, 3, 2);
        }

        public static Troop Blue(VectorTwo position)
        {
            return new Troop(PlayerSide.Blue, 5, position, 0, 2);
        }

        public static Troop Red(VectorTwo position, int orientation)
        {
            return new Troop(PlayerSide.Red, 5, position, orientation, 2);
        }

        public static Troop Blue(VectorTwo position, int orientation)
        {
            return new Troop(PlayerSide.Blue, 5, position, orientation, 2);
        }
    }
}
