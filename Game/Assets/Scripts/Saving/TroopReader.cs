using System.Collections.Generic;
using System.Linq;
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
                    return TroopFactory.Blue(dto.x, dto.y, dto.orientation);
                case "B17":
                    return TroopFactory.Red(new VectorTwo(dto.x, dto.y), dto.orientation);
                default:
                    return default;
            }
        }
    }
}