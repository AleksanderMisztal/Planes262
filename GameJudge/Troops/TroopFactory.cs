using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public static class TroopFactory
    {
        public static Fighter Red(int x, int y)
        {
            return new Fighter(PlayerSide.Red, 5, new VectorTwo(x, y), 3, 2);
        }

        public static Fighter Blue(int x, int y)
        {
            return new Fighter(PlayerSide.Blue, 5, new VectorTwo(x, y), 0, 2);
        }
        
        public static Fighter Blue(int x, int y, int movePoints)
        {
            return new Fighter(PlayerSide.Blue, movePoints, new VectorTwo(x, y), 0, 2);
        }

        public static Fighter Red(VectorTwo position)
        {
            return new Fighter(PlayerSide.Red, 5, position, 3, 2);
        }

        public static Fighter Blue(VectorTwo position)
        {
            return new Fighter(PlayerSide.Blue, 5, position, 0, 2);
        }

        public static Fighter Red(VectorTwo position, int orientation)
        {
            return new Fighter(PlayerSide.Red, 5, position, orientation, 2);
        }

        public static Fighter Blue(VectorTwo position, int orientation)
        {
            return new Fighter(PlayerSide.Blue, 5, position, orientation, 2);
        }
    }
}