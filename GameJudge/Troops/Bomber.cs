using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Bomber : Troop
    {
        protected override TroopType Type { get; set; } = TroopType.Bomber;

        public Bomber(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
        public Bomber() { }
        
        public override Troop Copy() => new Bomber(Player, initialMovePoints, Position, Orientation, Health);
    }
}