using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class ChatSentMessage : ServerMessage
    {
        public string message;

        public ChatSentMessage(string message) : base(ServerPackets.ChatSent)
        {
            this.message = message;
        }
    }
}