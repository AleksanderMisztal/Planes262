using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planes262.GameLogic;
using Assets.Scripts.UnityStuff;
using System.Collections.Generic;
using Planes262.Utils;

namespace GameTests
{
    [TestClass]
    public class GameControllerTests
    {

        private void Click(int x, int y)
        {
            var cell = new VectorTwo(x, y);
            MapController.OnCellClicked(cell);
        }

        [TestMethod]
        public void Should_BehaveWell_When_DoubleFight()
        {
            EventHandlers.OnGameJoined("Bot", PlayerSide.Blue, new Board(8, 5));
            List<Troop> troops = new List<Troop>
            {
                Troop.Blue(2, 2, 2),
                Troop.Blue(1, 3, 2),
                Troop.Red(3, 2),
                Troop.Red(4, 2),
            };
            EventHandlers.OnTroopsSpawned(troops);
            Click(2, 2);
            Click(3, 2);
            Click(3, 2);
        }
    }
}
