using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class LostOnTimeMessage : ServerMessage
    {
        public PlayerSide loser;
        
        public LostOnTimeMessage(PlayerSide loser) : base(ServerPackets.LostOnTime)
        {
            this.loser = loser;
        }
    }
}