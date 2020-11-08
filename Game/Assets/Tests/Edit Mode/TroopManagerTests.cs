using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using NUnit.Framework;
using Planes262.GameLogic;
using GameJudge.Troops;

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
            troopManager.MoveTroop(new VectorTwo(x, y), direction, battleResults.ToArray());
        }
        
        [Test]
        public void Should_HaveCorrectPositionAndHealth_When_AfterFight()
        {
            troopManager = new TroopManager(new TroopMap());
            troops = new List<Troop> {
                TroopFactory.Blue(1, 2).Get(),
                TroopFactory.Red(2, 2).Get(),
            };
            
            troopManager.BeginNextRound(troops);
            Move(1, 2, 0, 1);
            
            Assert.AreEqual(new VectorTwo(3, 1), troops[0].Position);
            Assert.IsFalse(troops[0].Destroyed);
        }

        [Test]
        public void TestNotCrashing()
        {
            troopManager = new TroopManager(new TroopMap());
            troops = new List<Troop> {
                TroopFactory.Blue(2, 3).Get(),
                TroopFactory.Red(6, 2).Get(),
                TroopFactory.Red(6, 3).Get(),
            };
            troopManager.BeginNextRound(troops);
            Move(2, 3, 0, 0);
            Move(3, 2, 0, 0);
            Move(4, 2, 0, 0);
        }
    }
}
