using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class GameEndedMessage : ServerMessage
    {
        public GameEndedMessage(ScoreInfo scoreInfo) : base(ServerPackets.GameEnded)
        {
            this.scoreInfo = scoreInfo;
        }
        
        public ScoreInfo scoreInfo;
    }
}