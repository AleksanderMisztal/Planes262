using NUnit.Framework;
using Planes262.Networking;
using Planes262.Networking.Packets;

namespace Planes262.Tests.Edit_Mode
{
    public class SerializerTests
    {
        [Test]
        public void Deserialize1()
        {
            string data = "1,0,0,0";
            byte[] bytes = Serializer.Deserialize(data);
            Packet packet = new Packet(bytes);
            int number = packet.ReadInt();
            Assert.AreEqual(1, number);
        }
    }
}