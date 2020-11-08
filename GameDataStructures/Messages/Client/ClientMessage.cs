using System;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public abstract class ClientMessage
    {
        public readonly ClientPackets type;

        protected ClientMessage(ClientPackets type)
        {
            this.type = type;
        }
    }
}