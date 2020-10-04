using GameDataStructures;

namespace GameJudge.Troops
{
    public class DamageScoringTroopDecorator : TroopDecorator
    {
        private readonly Score score;

        public DamageScoringTroopDecorator(Score score, ITroop troop) : base(troop)
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