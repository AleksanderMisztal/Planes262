using System.Collections.Generic;
using NUnit.Framework;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

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
            Assert.IsTrue(actual.Count == expected.Count);
            foreach (T item in expected)
                Assert.IsTrue(actual.Contains(item));
        }


        [Test]
        public void Should_ReturnControlZone_When_TroopHasOneMove()
        {
            List<Troop> troops = new List<Troop>
            {
                Troop.Blue(2, 2, 1)
            };
            CreatePathFinder(troops);

            HashSet<VectorTwo> cells = pathFinder.GetReachableCells(troops[0].Position);

            List<VectorTwo> expected = new List<VectorTwo>
            {
                new VectorTwo(2, 3),
                new VectorTwo(3, 2),
                new VectorTwo(2, 1),
            };

            AssertSetEquality(cells, expected);
        }

        [Test]
        public void Should_NotReachCell_When_OccupiedByFriend()
        {
            List<Troop> troops = new List<Troop>
            {
                Troop.Blue(2, 2, 2),
                Troop.Blue(2, 3)
            };
            CreatePathFinder(troops);

            HashSet<VectorTwo> cells = pathFinder.GetReachableCells(troops[0].Position);

            List<VectorTwo> expected = new List<VectorTwo>
            {
                new VectorTwo(3, 2),
                new VectorTwo(3, 3),
                new VectorTwo(4, 2),
                new VectorTwo(3, 1),
                new VectorTwo(2, 1),
                new VectorTwo(3, 0),
                new VectorTwo(2, 0),
            };
            AssertSetEquality(cells, expected);
        }

        [Test]
        public void Should_ReachReachableCells_When_DfsButNotBfsDoes()
        {
            List<Troop> troops = new List<Troop>
            {
                Troop.Blue(2, 2, 5)
            };
            CreatePathFinder(troops);

            HashSet<VectorTwo> cells = pathFinder.GetReachableCells(troops[0].Position);

            Assert.IsTrue(cells.Contains(new VectorTwo(7, 2)));
        }
    }
}
