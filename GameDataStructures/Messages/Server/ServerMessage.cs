using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public abstract class ServerMessage
    {
        public readonly ServerPackets type;

        protected ServerMessage(ServerPackets type)
        {
            this.type = type;
        }
    }
}