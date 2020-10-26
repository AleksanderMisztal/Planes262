using System.Collections.Generic;
using GameDataStructures;
using GameJudge;
using GameJudge.Troops;
using NUnit.Framework;

namespace JudgeTests
{
    public class TroopAiTests
    {
        private TroopMap troopMap;
        private TroopAi troopAi;

        private void MakeAi()
        {
            Board board = new Board(5, 5);
            troopMap = new TroopMap(board);
            troopAi = new TroopAi(troopMap, board);
        }


        [Test]
        public void Should_ReturnFalse_When_InMiddleBoard()
        {
            MakeAi();

            Troop troop = TroopFactory.Blue(2, 2);
            troopMap.SpawnWave(new List<Troop>
            {
                troop,
            });

            Assert.IsFalse(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnFalse_When_BlockedByFriends()
        {
            MakeAi();

            Troop troop = TroopFactory.Blue(0, 0);
            troopMap.SpawnWave(new List<Troop>
            {
                troop,
                TroopFactory.Blue(1, 0),
                TroopFactory.Blue(0, 1),
            });

            Assert.IsFalse(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnTrue_When_HasToExitBoard()
        {
            MakeAi();

            Troop troop = TroopFactory.Blue(5, 0);
            troopMap.SpawnWave(new List<Troop>
            {
                troop,
            });

            Assert.IsTrue(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnTrue_When_OutsideTheBoard()
        {
            MakeAi();

            Troop troop = TroopFactory.Blue(8, 2);
            troopMap.SpawnWave(new List<Troop>
            {
                troop,
            });

            Assert.IsTrue(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnTrue_When_CanReenterBoard()
        {
            MakeAi();

            Troop troop = TroopFactory.Blue(-1, 2);
            troopMap.SpawnWave(new List<Troop>
            {
                troop,
            });

            Assert.IsTrue(troopAi.ShouldControl(troop));
        }
    }
}
