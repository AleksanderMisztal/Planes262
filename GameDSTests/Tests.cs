using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
            ServerMessage message = new WelcomeMessage{message = "hello", position = new VectorTwo(3, 4)};
            MemoryStream ms = SerializeToStream(message);
            object dm = DeserializeFromStream(ms);
            ServerMessage m = (ServerMessage) dm;
            WelcomeMessage welcomeMessage = (WelcomeMessage) m;
            Trace.WriteLine("" + welcomeMessage.type + welcomeMessage.message + welcomeMessage.position);
        }
        
        public static MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public static object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object o = formatter.Deserialize(stream);
            return o;
        }
    }
}