using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Flak : Troop
    {
        public override TroopType Type { get; protected set; } = TroopType.Flak;

        public override IEnumerable<VectorTwo> ControlZone => Hex.GetNeighbours(Position);
        
        public Flak(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
        public Flak() { }
        
        public override Troop Copy() => new Flak(Player, initialMovePoints, Position, Orientation, Health);
    }
}