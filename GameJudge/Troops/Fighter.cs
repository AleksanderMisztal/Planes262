using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Fighter : Troop
    {
        public override TroopType Type { get; protected set; } = TroopType.Fighter;
        
        public Fighter(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
        public Fighter() { }
        
        public override Troop Copy() => new Fighter(Player, InitialMovePoints, Position, Orientation, Health);
    }
}