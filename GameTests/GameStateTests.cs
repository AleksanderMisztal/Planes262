using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameServer.GameLogic;

namespace GameTests
{
    [TestClass]
    public class GameStateTests
    {
        [TestMethod]
        public void Should_NotBeNull_When_Constructed()
        {
            Board board = new Board(5, 5);
            GameState gc = new GameState(board);

            Assert.IsNotNull(gc);
        }
    }
}
