using System;

namespace Planes262.GameLogic
{
    [Serializable]
    public class IllegalMoveException : Exception 
    {
        public IllegalMoveException(string message) : base($"Illegal move: {message}")
        {

        }
    }
}
