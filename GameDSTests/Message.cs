using System;
using GameDataStructures.Packets;

namespace GameDSTests
{
    [Serializable]
    public abstract class ServerMessage
    {
        public ServerPackets type;
    }
}