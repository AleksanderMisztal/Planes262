using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace Planes262.Saving
{
    public static class TroopReader
    {
        public static List<Troop> Load(string levelName)
        {
            TroopDtos troopDtos = Saver.Read<TroopDtos>(levelName + "/troops");
            return troopDtos.dtos.Select(Make).ToList();
        }


        private static Troop Make(TroopDto dto)
        {
            switch (dto.name)
            {
                case "Me262":
                    return new Fighter(PlayerSide.Blue, 5, new VectorTwo(dto.x, dto.y), dto.orientation, 2);
                case "B17":
                    return new Fighter(PlayerSide.Red, 5, new VectorTwo(dto.x, dto.y), dto.orientation, 2);
                default:
                    return default;
            }
        }
    }
}