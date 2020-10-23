using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using NUnit.Framework;
using Planes262.GameLogic;
using GameJudge.Troops;

namespace Planes262.Tests.Edit_Mode
{
    public class PathFinderTests
    {
        private TroopMap map;
        private PathFinder pathFinder;
        private Board board;

        private void CreatePathFinder(IEnumerable<Troop> troops)
        {
            board = new Board(10, 10);
            map = new TroopMap();
            map.SpawnWave(troops);
            pathFinder = new PathFinder(map, board);
        }

        private static void AssertSetEquality<T>(HashSet<T> actual, List<T> expected)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (T item in expected)
                Assert.IsTrue(actual.Contains(item), $"Didnt contain {item}");
        }


        [Test]
        public void Should_ReturnControlZone_When_TroopHasOneMove()
        {
            List<Troop> troops = new List<Troop>
            {
                TroopFactory.Blue(2, 2, 1),
            };
            CreatePathFinder(troops);

            HashSet<VectorTwo> cells = pathFinder.GetReachableCells(troops[0].Position);

            List<VectorTwo> expected = new List<VectorTwo>
            {
                new VectorTwo(2, 1),
                new VectorTwo(3, 1),
                new VectorTwo(3, 2),
            };

            AssertSetEquality(cells, expected);
        }

        [Test]
        public void Should_NotReachCell_When_OccupiedByFriend()
        {
            List<Troop> troops = new List<Troop>
            {
                TroopFactory.Blue(2, 2, 2),
                TroopFactory.Blue(3, 1),
            };
            CreatePathFinder(troops);

            HashSet<VectorTwo> cells = pathFinder.GetReachableCells(troops[0].Position);

            List<VectorTwo> expected = new List<VectorTwo>
            {
                new VectorTwo(3, 2),
                new VectorTwo(3, 3),
                new VectorTwo(4, 2),
                new VectorTwo(4, 3),
                new VectorTwo(2, 1),
                new VectorTwo(3, 0),
                new VectorTwo(2, 0),
                new VectorTwo(1, 0),
            };
            AssertSetEquality(cells, expected);
        }

        [Test]
        public void Should_ReachReachableCells_When_DfsButNotBfsDoes()
        {
            List<Troop> troops = new List<Troop>
            {
                TroopFactory.Blue(2, 2, 5)
            };
            CreatePathFinder(troops);

            HashSet<VectorTwo> cells = pathFinder.GetReachableCells(troops[0].Position);

            Assert.IsTrue(cells.Contains(new VectorTwo(7, 2)));
        }

        [Test]
        public void Should_ReturnCorrectCells_When_BoardLimits()
        {
            VectorTwo position = new VectorTwo(6, 2);
            List<Troop> troops = new List<Troop> {
                TroopFactory.Blue(position, 3),
            };
            board = Board.test;
            map = new TroopMap();
            map.SpawnWave(troops);
            pathFinder = new PathFinder(map, board);

            pathFinder.GetReachableCells(position);
            List<int> directions = pathFinder.GetDirections(position, new VectorTwo(6, 1));
            
            Assert.AreEqual(0, directions[0]);
            Assert.AreEqual(1, directions[1]);
            Assert.AreEqual(1, directions[2]);
            Assert.AreEqual(1, directions[3]);
            Assert.AreEqual(1, directions[4]);
            Assert.AreEqual(5, directions.Count);
        }
    }
}
