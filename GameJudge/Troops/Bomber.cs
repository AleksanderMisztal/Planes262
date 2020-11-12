using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public class Bomber : Troop
    {
        public override TroopType Type { get; } = TroopType.Bomber;
        public override bool CanAttack { get; } = true;

        public Bomber(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
            : base(player, movePoints, position, orientation, health) { }
    }
}