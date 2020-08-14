using GameServer.GameLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Board board = new Board(5, 5);
            GameController gc = new GameController(board);

            Assert.IsNotNull(gc);
        }
    }
}
