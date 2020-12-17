using System;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public class SendChatMessage : ClientMessage
    {
        public string message;

        public SendChatMessage(string message) : base(ClientPackets.SendMessage)
        {
            this.message = message;
        }
    }
}