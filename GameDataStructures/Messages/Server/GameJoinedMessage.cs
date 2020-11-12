using System;
using GameDataStructures.Dtos;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class GameJoinedMessage : ServerMessage
    {
        public GameJoinedMessage() : base(ServerPackets.GameJoined) { }
        
        public string opponentName;
        public PlayerSide side;
        public LevelDto levelDto;
        public ClockInfo clockInfo;
    }
}