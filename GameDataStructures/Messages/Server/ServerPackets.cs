using System;

namespace GameDataStructures.Messages.Server
{
    [Serializable]
    public enum ServerPackets
    {
        Welcome = 1,
        GameJoined = 2,
        TroopsSpawned = 3,
        TroopMoved = 4,
        GameEnded = 5,
        OpponentDisconnected = 6,
        ChatSent = 7,
        LostOnTime = 8,
    }
}