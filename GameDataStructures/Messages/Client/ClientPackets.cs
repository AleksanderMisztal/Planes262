using System;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public enum ClientPackets
    {
        JoinGame = 1,
        EndRound = 2,
        MoveTroop = 3,
        SendMessage = 4,
    }
}