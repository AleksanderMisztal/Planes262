using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace Planes262.Saving
{
    public static class TroopLoader
    {
        public static TroopDto[] Load(string levelName)
        {
            UTroopDtos uTroopDtos = Saver.Read<UTroopDtos>(levelName + "/troops");
            return uTroopDtos.dtos.Select(Make).ToArray();
        }


        private static TroopDto Make(UTroopDto dto)
        {
            switch (dto.name)
            {
                case "Me262":
                    return new TroopDto{type = TroopType.Fighter, side = PlayerSide.Blue, movePoints = 5, position = new VectorTwo(dto.x, dto.y), orientation = dto.orientation, health = 2};
                case "B17":
                    return new TroopDto{type = TroopType.Fighter, side = PlayerSide.Red, movePoints = 5, position = new VectorTwo(dto.x, dto.y), orientation = dto.orientation, health = 2};
                default:
                    return default;
            }
        }
    }
}