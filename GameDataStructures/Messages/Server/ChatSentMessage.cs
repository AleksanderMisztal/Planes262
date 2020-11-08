using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class ChatSentMessage : ServerMessage
    {
        public ChatSentMessage() : base(ServerPackets.ChatSent) { }

        public string message;
    }
}