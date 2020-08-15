using System;

namespace Assets.Scripts.GameLogic
{
    public class PathFindingException : Exception
    {
        public PathFindingException(string message) : base($"Couldn't find path: {message}") { }
    }
}
