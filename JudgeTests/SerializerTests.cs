using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using NUnit.Framework;

namespace JudgeTests
{
    public class SerializerTests
    {
        [Test]
        public void TestTroopSerialization()
        {
            Troop blue = TroopFactory.Blue(2, 3);

            string serialized = blue.Data;
            Assert.AreEqual("(0)(5)((2)(3))(0)(2)", serialized);
        }
        [Test]
        public void TestTroopDeserialization()
        {
            const string serialized = "(0)(5)((2)(3))(0)(2)";
            Troop troop = (Fighter)new Fighter().Read(serialized);
            
            Assert.AreEqual(PlayerSide.Blue, troop.Player);
            Assert.AreEqual(5, troop.MovePoints);
            Assert.AreEqual(new VectorTwo(2, 3), troop.Position);
            Assert.AreEqual(0, troop.Orientation);
            Assert.AreEqual(2, troop.Health);
        }

        [Test]
        public void TestDeserialization()
        {
            const string serialized = "(0)(5)((2)(3))(0)(2)";
            
            List<string> deserialized = Merger.Split(serialized);
            
            Assert.AreEqual(5, deserialized.Count);
            Assert.AreEqual("0", deserialized[0]);
            Assert.AreEqual("5", deserialized[1]);
            Assert.AreEqual("(2)(3)", deserialized[2]);
            Assert.AreEqual("0", deserialized[3]);
            Assert.AreEqual("2", deserialized[4]);
            
        }
    }
}