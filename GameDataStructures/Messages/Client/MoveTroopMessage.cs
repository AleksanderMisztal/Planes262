using System;
using GameDataStructures.Positioning;

namespace GameDataStructures.Messages.Client
{
    [Serializable]
    public class MoveTroopMessage : ClientMessage
    {
        public VectorTwo position;
        public int direction;

        public MoveTroopMessage(VectorTwo position, int direction) : base(ClientPackets.MoveTroop)
        {
            this.position = position;
            this.direction = direction;
        }
    }
}