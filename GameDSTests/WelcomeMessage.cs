using System;
using GameDataStructures.Positioning;

namespace GameDSTests
{
    [Serializable]
    public class WelcomeMessage : ServerMessage
    {
        public string message;
        public VectorTwo position;
    }
}