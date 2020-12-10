using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Bomber : Troop
    {
        public override TroopType Type { get; } = TroopType.Bomber;
        public override IEnumerable<VectorTwo> ControlZone => Hex.GetControlZone(Position, Orientation);

        public Bomber(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
    }
}