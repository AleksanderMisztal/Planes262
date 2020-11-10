using System.Collections.Generic;

namespace Planes262.UnityLayer
{
    public static class PersistState
    {
        public static string gameEndedMessage;

        public static IEnumerable<string> onlineLevels = new List<string>();
        public static IEnumerable<string> localLevels = new List<string>();
    }
}