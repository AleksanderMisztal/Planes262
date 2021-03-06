﻿using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Fighter : Troop
    {
        public override TroopType Type { get; } = TroopType.Fighter;
        public override IEnumerable<VectorTwo> ControlZone => Hex.GetControlZone(Position, Orientation);

        public Fighter(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }

        public Fighter(TroopDto dto) : base(dto) { }
    }
}