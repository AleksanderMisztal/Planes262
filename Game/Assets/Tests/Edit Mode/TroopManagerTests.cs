using System.Collections.Generic;
using GameJudge.Battles;
using NUnit.Framework;
using Planes262.GameLogic;
using Planes262.GameLogic.Troops;
using Planes262.GameLogic.Utils;

namespace Planes262.Tests.Edit_Mode
{
    public class TroopManagerTests
    {
        private TroopManager troopManager;
        private List<Troop> troops;

        private void Move(int x, int y, int direction, int battleCount)
        {
            List<BattleResult> battleResults = new List<BattleResult>();
            for (int i = 0; i < battleCount; i++) 
                battleResults.Add(new BattleResult(true, true));
            troopManager.MoveTroop(new VectorTwo(x, y), direction, battleResults);
        }
        
        [Test]
        public void Should_HaveCorrectPositionAndHealth_When_AfterFight()
        {
            troopManager = new TroopManager(new TroopMap(), new Score());
            troops = new List<Troop>
            {
                TroopFactory.Blue(1,1),
                TroopFactory.Blue(1,2),
                TroopFactory.Blue(1,3),
                TroopFactory.Red(4, 1),
                TroopFactory.Red(4, 2),
                TroopFactory.Red(4, 3),
            };
            
            troopManager.ResetForNewGame();
            troopManager.BeginNextRound(troops);
            Move(1, 1, 0, 0);
            Move(2, 1, 0, 0);
            Move(3, 1, 0, 1);
            
            Assert.IsTrue(troops[0].Position == new VectorTwo(5, 1));
            Assert.IsFalse(troops[0].Destroyed);
        }
        
        [Test]
        public void Should_ScoreCorrectly_When_FightsOccur()
        {
            Score score = new Score();
            troopManager = new TroopManager(new TroopMap(), score);
            troops = new List<Troop>
            {
                TroopFactory.Blue(1,1),
                TroopFactory.Blue(1,2),
                TroopFactory.Blue(1,3),
                TroopFactory.Red(4, 1),
                TroopFactory.Red(4, 2),
                TroopFactory.Red(4, 3),
            };
            
            troopManager.ResetForNewGame();
            troopManager.BeginNextRound(troops);
            Move(1, 1, 0, 0);
            Move(2, 1, 0, 0);
            Move(3, 1, 0, 1);
            
            Assert.AreEqual("1 : 1", score.ToString());
        }
    }
}
