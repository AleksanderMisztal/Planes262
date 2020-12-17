using System;
using GameDataStructures.Dtos;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class GameJoinedMessage : ServerMessage
    {
        public string opponentName;
        public PlayerSide side;
        public LevelDto levelDto;
        public ClockInfo clockInfo;

        public GameJoinedMessage(string opponentName, PlayerSide side, LevelDto levelDto, ClockInfo clockInfo) : base(ServerPackets.GameJoined)
        {
            this.opponentName = opponentName;
            this.side = side;
            this.levelDto = levelDto;
            this.clockInfo = clockInfo;
        }
    }
}