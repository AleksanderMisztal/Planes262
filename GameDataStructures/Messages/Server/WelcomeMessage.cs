using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class WelcomeMessage : ServerMessage
    {
        public string[] gameTypes;
        
        public WelcomeMessage(string[] gameTypes) : base(ServerPackets.Welcome)
        {
            this.gameTypes = gameTypes;
        }
    }
}