using System.Collections.Generic;
using System.Diagnostics;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Battles;
using GameJudge.Troops;
using GameJudge.Waves;
using NUnit.Framework;

namespace JudgeTests
{
    public class GameControllerTests
    {
        private GameController gc;

        private void CreateGameController(WaveProvider waveProvider, int xSize, int ySize)
        {
            IBattleResolver battles = new AlwaysDamageBattles();
            Board board = new Board(xSize, ySize);

            gc = new GameController(battles, board, waveProvider);
        }

        [Test]
        public void Should_ControlTroopWithAI_When_ExitsBoard()
        {
            WaveProvider waveProvider = new WaveProvider(new List<Troop>
            {
                new Fighter(PlayerSide.Blue, 5, new VectorTwo(2, 3), 2, 2 ),
                new Fighter(PlayerSide.Red, 5, new VectorTwo(2, 4), 5, 2 ),
            });
            CreateGameController(waveProvider, 5, 5);
            int moveCount = 0;
            gc.TroopMoved += args => moveCount++;


            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 3), 0);

            Assert.AreEqual(4, moveCount);
        }

        [Test]
        public void Should_AllowEnteringFriend_When_Blocked()
        {
            Troop troop = TroopFactory.Blue(2, 2);
            WaveProvider waveProvider = new WaveProvider(new List<Troop>
            {
                troop,
                TroopFactory.Blue(3, 2),
                TroopFactory.Blue(3, 1),
                TroopFactory.Blue(2, 1),
                TroopFactory.Red(8, 3),
            });

            CreateGameController(waveProvider, 10, 10);
            
            int moveCount = 0;
            gc.TroopMoved += args => moveCount++;

            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 2), 0);

            Assert.AreEqual(1, moveCount);
        }

        [Test]
        public void TestReturningCopies()
        {
            WaveProvider waveProvider = new WaveProvider(new List<Troop>{TroopFactory.Blue(2, 2)}, null);
            gc = new GameController(waveProvider, new Board(5, 5));
            Troop troop = waveProvider.initialTroops[0];
            VectorTwo position = troop.Position;
            
            gc.ProcessMove(PlayerSide.Blue, position, 0);
            
            Assert.AreEqual(position, troop.Position);
        }

        [Test]
        public void ShouldNotControlDestroyedTroop()
        {
            WaveProvider waveProvider = new WaveProvider(new List<Troop>
            {
                TroopFactory.Blue(new VectorTwo(4, 6), 2),
                TroopFactory.Red(4, 7),
                TroopFactory.Red(4, 9),
            });

            CreateGameController(waveProvider, 10, 10);
            
            int moveCount = 0;
            gc.TroopMoved += args => moveCount++;

            
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(4, 6), 0);
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(4, 8), 0);
            
            Assert.AreEqual(2, moveCount);
        }
    }
}
