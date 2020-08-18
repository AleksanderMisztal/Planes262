﻿using System.Collections.Generic;
using Assets.Scripts.UnityStuff;
using NUnit.Framework;
using Planes262.GameLogic;
using Planes262.Utils;

namespace GameTests
{
    public class GameControllerTests
    {

        private void Click(int x, int y)
        {
            var cell = new VectorTwo(x, y);
            MapController.OnCellClicked(cell);
        }

        [Test]
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
