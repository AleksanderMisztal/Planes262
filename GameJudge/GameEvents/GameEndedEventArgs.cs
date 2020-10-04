using System;
using GameDataStructures;

namespace GameJudge.GameEvents
{
    public class GameEndedEventArgs : EventArgs
    {
        public readonly ScoreInfo Score;

        internal GameEndedEventArgs(Score score)
        {
            Score = new ScoreInfo(score.Red, score.Blue);
        }

        public override string ToString()
        {
            return $"Game ended event\nRed: {Score.Red}, blue: {Score.Blue}";
        }
    }
}
