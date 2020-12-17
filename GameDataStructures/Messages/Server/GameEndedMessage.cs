using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class GameEndedMessage : ServerMessage
    {
        public ScoreInfo scoreInfo;
        
        public GameEndedMessage(ScoreInfo scoreInfo) : base(ServerPackets.GameEnded)
        {
            this.scoreInfo = scoreInfo;
        }
    }
}