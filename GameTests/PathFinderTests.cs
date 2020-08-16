using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameServer.GameLogic;
using System.Collections.Generic;
using Assets.Scripts.GameLogic;
using GameServer.Utils;

namespace GameTests
{
    [TestClass]
    public class PathFinderTest
    {
        private TroopMap map;
        private PathFinder pathFinder;

        private void CreatePathFinder(List<Troop> troops)
        {
            map = new TroopMap();
            map.SpawnWave(troops);
            pathFinder = new PathFinder(map);
        }

        private void AssertSetEquality<T>(HashSet<T> actual, List<T> expected)
        {
            Assert.IsTrue(actual.Count == expected.Count);
            foreach (var troop in expected)
                Assert.IsTrue(actual.Contains(troop));
        }


        [TestMethod]
        public void Should_ReturnControllZone_When_TroopHasOneMove()
        {
            List<Troop> troops = new List<Troop>
            {
                Troop.Blue(2, 2, 1)
            };
            CreatePathFinder(troops);

            var cells = pathFinder.GetReachableCells(troops[0].Position);

            List<VectorTwo> expected = new List<VectorTwo>
            {
                new VectorTwo(2, 3),
                new VectorTwo(3, 2),
                new VectorTwo(2, 1),
            };

            AssertSetEquality(cells, expected);
        }

        [TestMethod]
        public void Should_NotReachCell_When_OccupiedByFriend()
        {
            List<Troop> troops = new List<Troop>
            {
                Troop.Blue(2, 2, 2),
                Troop.Blue(2, 3)
            };
            CreatePathFinder(troops);

            var cells = pathFinder.GetReachableCells(troops[0].Position);

            List<VectorTwo> expected = new List<VectorTwo>
            {
                new VectorTwo(3, 2),
                new VectorTwo(4, 3),
                new VectorTwo(4, 2),
                new VectorTwo(3, 1),
                new VectorTwo(2, 1),
                new VectorTwo(3, 0),
                new VectorTwo(2, 0),
            };
            AssertSetEquality(cells, expected);
        }


    }
}
