using GameDataStructures;
using GameDataStructures.Positioning;
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

            troopMap.SpawnWave(new[]{TroopFactory.Blue(2, 2),});

            
            Assert.IsFalse(troopAi.ShouldControl(troopMap.Get(new VectorTwo(2, 2))));
        }

        [Test]
        public void Should_ReturnFalse_When_BlockedByFriends()
        {
            MakeAi();
            troopMap.SpawnWave(new[]
            {
                TroopFactory.Blue(0, 0),
                TroopFactory.Blue(1, 0),
                TroopFactory.Blue(0, 1),
            });
            Assert.IsFalse(troopAi.ShouldControl(troopMap.Get(new VectorTwo(0, 0))));
        }

        [Test]
        public void Should_ReturnTrue_When_HasToExitBoard()
        {
            MakeAi();
            troopMap.SpawnWave(new[]{TroopFactory.Blue(5, 0),});
            Assert.IsTrue(troopAi.ShouldControl(troopMap.Get(new VectorTwo(5, 0))));
        }

        [Test]
        public void Should_ReturnTrue_When_OutsideTheBoard()
        {
            MakeAi();
            troopMap.SpawnWave(new[]{TroopFactory.Blue(8, 2),});
            Assert.IsTrue(troopAi.ShouldControl(troopMap.Get(new VectorTwo(8, 2))));
        }

        [Test]
        public void Should_ReturnTrue_When_CanReenterBoard()
        {
            MakeAi();
            troopMap.SpawnWave(new[]{TroopFactory.Blue(-1, 2),});
            Assert.IsTrue(troopAi.ShouldControl(troopMap.Get(new VectorTwo(-1, 2))));
        }
    }
}
