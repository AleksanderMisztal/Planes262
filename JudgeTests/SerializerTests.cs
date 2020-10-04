using GameDataStructures.Packets;
using NUnit.Framework;

namespace JudgeTests
{
    public class SerializerTests
    {
        [Test]
        public void Deserialize1()
        {
            const string data = "1,0,0,0";
            byte[] bytes = Serializer.Deserialize(data);
            Packet packet = new Packet(bytes);
            int number = packet.ReadInt();
            Assert.AreEqual(1, number);
        }
    }
}