using System;
using System.Collections.Generic;
using GameDataStructures.Battles;
using GameDataStructures.Positioning;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class TroopMovedMessage : ServerMessage
    {
        public readonly VectorTwo position;
        public readonly int direction;
        public readonly BattleResult[] battleResults;
        public readonly ScoreInfo scoreInfo;
        
        public TroopMovedMessage(VectorTwo position, int direction, 
            BattleResult[] battleResults, ScoreInfo scoreInfo)
            : base(ServerPackets.TroopMoved)
        {
            this.position = position;
            this.direction = direction;
            this.battleResults = battleResults;
            this.scoreInfo = scoreInfo;
        }
    }
}