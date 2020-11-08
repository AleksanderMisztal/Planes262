using System;
using GameDataStructures.Positioning;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class TroopMovedMessage : ServerMessage
    {
        public TroopMovedMessage() : base(ServerPackets.TroopMoved) { }

        public VectorTwo position;
        public int direction;
        public BattleResult[] battleResults;
        public ScoreInfo scoreInfo;
    }
}