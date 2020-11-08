using System;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public class JoinGameMessage : ClientMessage
    {
        public JoinGameMessage() : base(ClientPackets.JoinGame) { }

        public string username;
        public string gameType;
    }
}