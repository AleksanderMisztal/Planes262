using System;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public class SendChatMessage : ClientMessage
    {
        public SendChatMessage() : base(ClientPackets.SendMessage) { }

        public string message;
    }
}