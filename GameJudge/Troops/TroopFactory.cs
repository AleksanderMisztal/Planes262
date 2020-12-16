using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public static class TroopFactory
    {
        public static TroopDto Red(int x, int y)
        {
            return new TroopDto("Fighter", TroopType.Fighter,PlayerSide.Red, new V2Dto(x, y),3,5, 2);
        }

        public static TroopDto Blue(int x, int y)
        {
            return new TroopDto("Fighter", TroopType.Fighter,PlayerSide.Blue, new V2Dto(x, y),0,5, 2);
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

        public static TroopDto Blue(VectorTwo position, int orientation)
        {
            return new TroopDto("Fighter", TroopType.Fighter, PlayerSide.Blue, position.Dto(), orientation,5, 2);
        }
        
        public static TroopDto Red(VectorTwo position, int orientation)
        {
            return new TroopDto("Fighter", TroopType.Fighter, PlayerSide.Red, position.Dto(), orientation,5, 2);
        }
    }
}