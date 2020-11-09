using System;
using GameDataStructures.Dtos;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public class TroopsSpawnedMessage : ServerMessage
    {
        public TroopsSpawnedMessage() : base(ServerPackets.TroopsSpawned) { }

        public TroopDto[] troops;
        public TimeInfo timeInfo;
    }
}