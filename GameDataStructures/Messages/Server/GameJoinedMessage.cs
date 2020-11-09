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
        public Board board;
        public TroopDto[] troops;
        public ClockInfo clockInfo;
    }
}