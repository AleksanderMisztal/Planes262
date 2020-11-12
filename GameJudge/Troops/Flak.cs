using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Flak : Troop
    {
        public override TroopType Type { get;}  = TroopType.Flak;
        public override bool CanAttack { get; } = false;

        public override IEnumerable<VectorTwo> ControlZone => Hex.GetNeighbours(Position);
        
        public Flak(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
        
        public Flak(TroopDto dto) : base(dto) { }
        
        public override void MoveInDirection(int direction)
        {
            MovePoints--;
            Position = Hex.GetAdjacentHex(Position, direction);
        }
    }
}