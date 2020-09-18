using GameDataStructures;
using Planes262.GameLogic.Area;

namespace Planes262.GameLogic.Troops
{
    public class AreaScoringTroopDecorator : TroopDecorator
    {
        private readonly Score score;
        private readonly IArea area;
        private readonly int amount;
        private bool entered = false;

        public AreaScoringTroopDecorator(Score score, IArea area, int amount, ITroop troop) : base(troop)
        {
            this.score = score;
            this.area = area;
            this.amount = amount;
        }

        public override void MoveInDirection(int direction)
        {
            base.MoveInDirection(direction);
            IncrementIfInArea();
        }

        public override void FlyOverOtherTroop()
        {
            base.FlyOverOtherTroop();
            IncrementIfInArea();
        }

        private void IncrementIfInArea()
        {
            if (!area.IsInside(Position) || entered) return;
            
            entered = true;
            score.Increment(Player, amount);
        }
    }
}