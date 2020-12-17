using System;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public class JoinGameMessage : ClientMessage
    {
        public JoinGameMessage(string username, string gameType) : base(ClientPackets.JoinGame)
        {
            this.username = username;
            this.gameType = gameType;
        }

        public string username;
        public string gameType;
    }
}