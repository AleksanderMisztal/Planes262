using System.IO;
using GameDataStructures.Messages;
using GameDataStructures.Messages.Server;
using GameDataStructures.Positioning;
using NUnit.Framework;

namespace GameDSTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test()
        {
            TestMessage message = new TestMessage{message = "hello", position = new VectorTwo(3, 4)};
            MemoryStream ms = Serializer.SerializeToStream(message);
            object dm = Serializer.DeserializeFromStream(ms);
            ServerMessage m = (ServerMessage) dm;
            TestMessage testMessage = (TestMessage) m;
            
            Assert.AreEqual(message.type, testMessage.type);
            Assert.AreEqual(message.message, testMessage.message);
            Assert.AreEqual(message.position, testMessage.position);
        }
    }
}