using System;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public class EndRoundMessage : ClientMessage
    {
        public EndRoundMessage() : base(ClientPackets.EndRound) { }
    }
}