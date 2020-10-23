using System;
using GameDataStructures;

namespace GameJudge.GameEvents
{
    public class GameEndedEventArgs : EventArgs
    {
        public readonly ScoreInfo score;

        internal GameEndedEventArgs(Score score)
        {
            this.score = new ScoreInfo(score.Red, score.Blue);
        }

        public override string ToString()
        {
            return $"Game ended event\nRed: {score.Red}, blue: {score.Blue}";
        }
    }
}
