using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Troops;
using NUnit.Framework;

namespace JudgeTests
{
    public class MoveValidatorTests
    {
        private TroopMap troopMap;
        private MoveValidator validator;

        
        private void MakeValidator()
        {
            Board board = new Board(5, 5);
            troopMap = new TroopMap(board);
            validator = new MoveValidator(troopMap, board, PlayerSide.Blue);
        }

        private void AssertForwardLegality(bool legal, int x, int y)
        {
            Assert.AreEqual(legal, validator.IsLegalMove(PlayerSide.Blue, new VectorTwo(x, y), 0));
        }
        

        [Test]
        public void ShouldMove()
        {
            MakeValidator();

            troopMap.SpawnWave(new List<Troop>
            {
                TroopFactory.Blue(2, 2),
            });

            AssertForwardLegality(true, 2, 2);
        }

        [Test]
        public void ShouldNotEnterFriend()
        {
            MakeValidator();

            troopMap.SpawnWave(new List<Troop>
            {
                TroopFactory.Blue(2, 2),
                TroopFactory.Blue(3, 1),
            });

            AssertForwardLegality(false, 2, 2);
        }

        [Test]
        public void Friends3NotBlocking()
        {
            MakeValidator();

            troopMap.SpawnWave(new List<Troop>()
            {
                TroopFactory.Blue(2, 2),
                TroopFactory.Blue(3, 2),
                TroopFactory.Blue(3, 1),
                TroopFactory.Blue(2, 1),
            });

            AssertForwardLegality(true, 2, 2);
        }

        [Test]
        public void ShouldNotExitBoard()
        {
            MakeValidator();

            troopMap.SpawnWave(new List<Troop>()
            {
                TroopFactory.Blue(5, 0),
            });

            AssertForwardLegality(false, 5, 2);
        }
    }
}
