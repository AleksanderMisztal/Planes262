using System;

namespace GameDataStructures
{
    [Serializable]
    public struct ClockInfo
    {
        public int InitialTimeS { get; private set; }
        public int IncrementS { get; private set; }
        public long StartTimestamp { get; private set; }

        public ClockInfo(int initialTimeS, int incrementS, long startTimestamp)
        {
            InitialTimeS = initialTimeS;
            IncrementS = incrementS;
            StartTimestamp = startTimestamp;
        }
    }
}