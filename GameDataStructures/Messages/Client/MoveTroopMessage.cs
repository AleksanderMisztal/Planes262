using System;
using GameDataStructures.Positioning;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public class MoveTroopMessage : ClientMessage
    {
        public MoveTroopMessage() : base(ClientPackets.MoveTroop) { }

        public VectorTwo position;
        public int direction;
    }
}