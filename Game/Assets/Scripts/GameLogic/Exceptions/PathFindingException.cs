using System;

namespace Planes262.GameLogic.Exceptions
{
    [Serializable]
    public class PathFindingException : Exception
    {
        public PathFindingException(string message) : base($"Couldn't find path: {message}") { }
    }
}
