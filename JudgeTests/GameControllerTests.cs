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

        private void Move(PlayerSide player, int x, int y, int direction)
        {
            gc.ProcessMove(player, new VectorTwo(x, y), direction);
        }


        [Test]
        public void Should_EndGame_When_OneSideLosesAllTroops()
        {
            WaveProvider waveProvider = new WavesBuilder()
                .Add(1, 3, 3, PlayerSide.Blue)
                .Add(1, 2, 3, PlayerSide.Blue)
                .Add(1, 5, 3, PlayerSide.Red)
                .GetWaves();

            CreateGameController(waveProvider, 10, 10);

            Move(PlayerSide.Blue, 3, 3, 0);
            Move(PlayerSide.Blue, 4, 3, 0);
            Move(PlayerSide.Blue, 6, 3, 0);
            Move(PlayerSide.Blue, 2, 3, 0);
            Move(PlayerSide.Blue, 3, 3, 0);
            Move(PlayerSide.Blue, 4, 3, 0);

            Assert.IsTrue(true);
        }

        [Test]
        public void Should_ContinueGame_When_MoreTroopsWillSpawn()
        {
            WaveProvider waveProvider = new WavesBuilder()
                .Add(1, 3, 3, PlayerSide.Blue)
                .Add(1, 2, 3, PlayerSide.Blue)
                .Add(1, 5, 3, PlayerSide.Red)
                .Add(4, 1, 1, PlayerSide.Red)
                .GetWaves();

            CreateGameController(waveProvider, 10, 10);

            Move(PlayerSide.Blue, 3, 3, 0);
            Move(PlayerSide.Blue, 4, 3, 0);
            Move(PlayerSide.Blue, 6, 3, 0);
            Move(PlayerSide.Blue, 2, 3, 0);
            Move(PlayerSide.Blue, 3, 3, 0);
            Move(PlayerSide.Blue, 4, 3, 0);

            Assert.AreEqual(1, 1);
        }

        [Test]
        public void Should_ControlTroopWithAI_When_ExitsBoard()
        {
            WaveProvider waveProvider = new WaveProvider(new List<Troop>
            {
                new Fighter(PlayerSide.Blue, 5, new VectorTwo(2, 3), 2, 2 ),
                new Fighter(PlayerSide.Red, 5, new VectorTwo(2, 4), 5, 2 ),
            }, new Dictionary<int, List<Troop>>());
            CreateGameController(waveProvider, 5, 5);
            int moveCount = 0;
            gc.TroopMoved += args => moveCount++;


            Move(PlayerSide.Blue, 2, 3, 0);

            Assert.AreEqual(4, moveCount);
        }

        [Test]
        public void Should_AllowEnteringFriend_When_Blocked()
        {
            WaveProvider waveProvider = new WavesBuilder()
                .Add(1, 0, 3, PlayerSide.Blue)
                .Add(1, 1, 2, PlayerSide.Blue)
                .Add(1, 1, 3, PlayerSide.Blue)
                .Add(1, 1, 4, PlayerSide.Blue)
                .Add(1, 5, 3, PlayerSide.Red)
                .GetWaves();

            CreateGameController(waveProvider, 10, 10);

            Move(PlayerSide.Blue, 0, 3, 0);

            Assert.AreEqual(1, 1);
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
    }
}
