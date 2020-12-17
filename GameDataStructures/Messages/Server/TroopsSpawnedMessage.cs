using System;
using GameDataStructures.Dtos;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class TroopsSpawnedMessage : ServerMessage
    {
        public TroopDto[] troops;
        public TimeInfo timeInfo;
        
        public TroopsSpawnedMessage(TroopDto[] troops, TimeInfo timeInfo) : base(ServerPackets.TroopsSpawned)
        {
            this.troops = troops;
            this.timeInfo = timeInfo;
        }
    }
}