using GameDataStructures.Positioning;
using NUnit.Framework;

namespace Planes262.Tests.Edit_Mode
{
    public class HexLogicTests
    {
        [Test]
        public void TestGoingInDirectionsOdd()
        {
            VectorTwo oneOne = new VectorTwo(1, 1);

            VectorTwo dir0 = Hex.GetAdjacentHex(oneOne, 0);
            VectorTwo dir1 = Hex.GetAdjacentHex(oneOne, 1);
            VectorTwo dir2 = Hex.GetAdjacentHex(oneOne, 2);
            VectorTwo dir3 = Hex.GetAdjacentHex(oneOne, 3);
            VectorTwo dir4 = Hex.GetAdjacentHex(oneOne, 4);
            VectorTwo dir5 = Hex.GetAdjacentHex(oneOne, 5);
            
            Assert.AreEqual(new VectorTwo(2, 1), dir0);
            Assert.AreEqual(new VectorTwo(2, 2), dir1);
            Assert.AreEqual(new VectorTwo(1, 2), dir2);
            Assert.AreEqual(new VectorTwo(0, 2), dir3);
            Assert.AreEqual(new VectorTwo(0, 1), dir4);
            Assert.AreEqual(new VectorTwo(1, 0), dir5);
        }
        
        [Test]
        public void TestGoingInDirectionsEven()
        {
            VectorTwo oneOne = new VectorTwo(2, 1);

            VectorTwo dir0 = Hex.GetAdjacentHex(oneOne, 0);
            VectorTwo dir1 = Hex.GetAdjacentHex(oneOne, 1);
            VectorTwo dir2 = Hex.GetAdjacentHex(oneOne, 2);
            VectorTwo dir3 = Hex.GetAdjacentHex(oneOne, 3);
            VectorTwo dir4 = Hex.GetAdjacentHex(oneOne, 4);
            VectorTwo dir5 = Hex.GetAdjacentHex(oneOne, 5);
            
            Assert.AreEqual(new VectorTwo(3, 0), dir0);
            Assert.AreEqual(new VectorTwo(3, 1), dir1);
            Assert.AreEqual(new VectorTwo(2, 2), dir2);
            Assert.AreEqual(new VectorTwo(1, 1), dir3);
            Assert.AreEqual(new VectorTwo(1, 0), dir4);
            Assert.AreEqual(new VectorTwo(2, 0), dir5);
        }
    }
}