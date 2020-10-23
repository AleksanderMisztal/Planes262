using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace GameJudge.GameEvents
{
    public class TroopMovedEventArgs : EventArgs
    {
        public readonly VectorTwo position;
        public readonly int direction;
        public readonly List<BattleResult> battleResults;
        public readonly ScoreInfo score;

        public TroopMovedEventArgs(VectorTwo position, int direction, List<BattleResult> battleResults, Score score)
        {
            this.position = position;
            this.direction = direction;
            this.battleResults = battleResults;
            this.score = new ScoreInfo(score.Red, score.Blue);
        }

        public override string ToString()
        {
            string res = $"Troop moved event\n p: {position} d: {direction}\n";
            return battleResults.Aggregate(res, (current, b) => current + (b + "\n"));
        }
    }
}
