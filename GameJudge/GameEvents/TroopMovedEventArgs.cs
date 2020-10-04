using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.GameEvents
{
    public class TroopMovedEventArgs : EventArgs
    {
        public readonly VectorTwo Position;
        public readonly int Direction;
        public readonly List<BattleResult> BattleResults;
        public readonly ScoreInfo Score;

        public TroopMovedEventArgs(VectorTwo position, int direction, List<BattleResult> battleResults, Score score)
        {
            Position = position;
            Direction = direction;
            BattleResults = battleResults;
            Score = new ScoreInfo(score.Red, score.Blue);
        }

        public override string ToString()
        {
            string res = $"Troop moved event\n p: {Position} d: {Direction}\n";
            return BattleResults.Aggregate(res, (current, b) => current + (b + "\n"));
        }
    }
}
