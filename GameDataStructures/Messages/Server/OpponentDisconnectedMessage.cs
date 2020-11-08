using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class OpponentDisconnectedMessage : ServerMessage
    {
        public OpponentDisconnectedMessage() : base(ServerPackets.OpponentDisconnected) { }
    }
}