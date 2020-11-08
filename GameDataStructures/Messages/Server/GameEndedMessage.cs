using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class GameEndedMessage : ServerMessage
    {
        public GameEndedMessage() : base(ServerPackets.GameEnded) { }

        public ScoreInfo scoreInfo;
    }
}