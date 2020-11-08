using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class LostOnTimeMessage : ServerMessage
    {
        public LostOnTimeMessage() : base(ServerPackets.LostOnTime) { }

        public PlayerSide loser;
    }
}