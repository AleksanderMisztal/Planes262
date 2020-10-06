using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using NUnit.Framework;
using Planes262.GameLogic;
using GameJudge.Troops;
using GameJudge.Waves;

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
            troopManager = new TroopManager(new TroopMap());
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
        public void TestNotCrashing()
        {
            troopManager = new TroopManager(new TroopMap());
            troops = new List<Troop>
            {
                TroopFactory.Blue(2, 3),
                TroopFactory.Red(6, 2),
                TroopFactory.Red(6, 3),
            };
            troopManager.ResetForNewGame();
            troopManager.BeginNextRound(troops);
            Move(2, 3, 0, 0);
            Move(3, 3, 0, 0);
            Move(4, 3, 0, 0);
        }
    }
}
