using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Bomber : Troop
    {
        public override TroopType Type { get; protected set; } = TroopType.Bomber;

        public Bomber(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
        public Bomber() { }
        
        public override Troop Copy() => new Bomber(Player, initialMovePoints, Position, Orientation, Health);
    }
}