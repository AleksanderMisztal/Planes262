using GameServer.GameLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTests
{
    [TestClass]
    public class GameControllerTests
    {
        [TestMethod]
        public void Should_NotBeNull_When_Constructed()
        {
            Board board = new Board(5, 5);
            GameController gc = new GameController(board);

            Assert.IsNotNull(gc);
        }
    }
}
