using GameDataStructures;
using GameDataStructures.Dtos;

namespace GameJudge.Troops
{
    public static class TroopReader
    {
        public static Troop Get(this TroopDto dto)
        {
            switch (dto.type)
            {
                case TroopType.Fighter:
                    return new Fighter(dto.side, dto.movePoints, dto.position.Get(), dto.orientation, dto.health);
                case TroopType.Flak:
                    return new Flak(dto.side, dto.movePoints, dto.position.Get(), dto.orientation, dto.health);
                default:
                    return new Bomber(dto.side, dto.movePoints, dto.position.Get(), dto.orientation, dto.health);
            }
        }
    }
}