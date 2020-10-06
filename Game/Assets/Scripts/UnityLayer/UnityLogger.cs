using GameDataStructures;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class UnityLogger : IMyLogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
    }
}