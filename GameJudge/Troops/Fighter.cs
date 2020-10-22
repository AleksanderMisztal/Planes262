using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Fighter : Troop
    {
        protected override TroopType Type { get; set; } = TroopType.Fighter;
        
        public Fighter(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
        public Fighter() { }
        
        public override Troop Copy() => new Fighter(Player, initialMovePoints, Position, Orientation, Health);
    }
}