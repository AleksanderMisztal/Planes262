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
        private const int forward = 0;

        private GameController gc;

        private void CreateGameController(WaveProvider waveProvider, int xMax, int yMax)
        {
            IBattleResolver battles = new AlwaysDamageBattles();
            Board board = new Board(xMax, yMax);

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

            Move(PlayerSide.Blue, 3, 3, forward);
            Move(PlayerSide.Blue, 4, 3, forward);
            Move(PlayerSide.Blue, 6, 3, forward);
            Move(PlayerSide.Blue, 2, 3, forward);
            Move(PlayerSide.Blue, 3, 3, forward);
            Move(PlayerSide.Blue, 4, 3, forward);

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

            Move(PlayerSide.Blue, 3, 3, forward);
            Move(PlayerSide.Blue, 4, 3, forward);
            Move(PlayerSide.Blue, 6, 3, forward);
            Move(PlayerSide.Blue, 2, 3, forward);
            Move(PlayerSide.Blue, 3, 3, forward);
            Move(PlayerSide.Blue, 4, 3, forward);

            Assert.AreEqual(1, 1);
        }

        [Test]
        public void Should_ControlTroopWithAI_When_ExitsBoard()
        {
            WaveProvider waveProvider = new WavesBuilder()
                .Add(1, 4, 3, PlayerSide.Blue)
                .Add(1, 5, 3, PlayerSide.Red)
                .GetWaves();

            CreateGameController(waveProvider, 5, 5);

            Move(PlayerSide.Blue, 4, 3, forward);

            Assert.IsTrue(true);
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

            Move(PlayerSide.Blue, 0, 3, forward);

            Assert.AreEqual(1, 1);
        }

        [Test]
        public void TestReturningCopies()
        {
            WaveProvider waveProvider = WaveProvider.Basic();
            gc = new GameController(waveProvider, Board.test);
            Troop troop = waveProvider.initialTroops[0];
            VectorTwo position = troop.Position;
            
            gc.ProcessMove(PlayerSide.Blue, position, forward);
            
            Assert.AreEqual(position, troop.Position);
        }
    }
}
