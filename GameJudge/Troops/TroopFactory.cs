using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public static class TroopFactory
    {
        public static TroopDto Red(int x, int y)
        {
            return new TroopDto{type = TroopType.Fighter, side = PlayerSide.Red, movePoints = 5, position = new VectorTwo(x, y), orientation = 3, health = 2};
        }

        public static TroopDto Blue(int x, int y)
        {
            return new TroopDto{type = TroopType.Fighter, side = PlayerSide.Blue, movePoints = 5, position = new VectorTwo(x, y), orientation = 0, health = 2};
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

        public static TroopDto Blue(VectorTwo position, int orientation)
        {
            return new TroopDto{type = TroopType.Fighter, side = PlayerSide.Red, movePoints = 5, position = position, orientation = orientation, health = 2};
        }
    }
}