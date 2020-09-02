namespace Planes262.GameLogic.Troops
{
    public class DamageScoringTroopDecorator : TroopDecorator
    {
        private readonly Score score;

        public DamageScoringTroopDecorator(ITroop troop, Score score) : base(troop)
        {
            this.score = score;
        }

        public override void ApplyDamage()
        {
            base.ApplyDamage();
            score.Increment(Player.Opponent());
        }
    }
}