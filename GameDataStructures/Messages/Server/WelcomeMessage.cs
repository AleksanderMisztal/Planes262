using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class WelcomeMessage : ServerMessage
    {
        public WelcomeMessage() : base(ServerPackets.Welcome) { }

        public string[] gameTypes;
    }
}