using System.Collections.Generic;
using GameDataStructures;
using GameJudge.Troops;

namespace Planes262.UnityLayer
{
    public static class PersistState
    {
        public static bool isLocal = false;
        public static string gameEndedMessage;
        
        public static string opponentName;
        public static PlayerSide side;
        public static Board board;
        public static ClockInfo clockInfo;
        public static IEnumerable<Troop> troops;
    }
}