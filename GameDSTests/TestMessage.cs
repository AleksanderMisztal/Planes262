using System;
using GameDataStructures.Messages.Server;
using GameDataStructures.Positioning;

namespace GameDSTests
{
    [Serializable]
    public class TestMessage : ServerMessage
    {
        public string message;
        public VectorTwo position;

        public TestMessage() : base(ServerPackets.Welcome) { }
    }
}