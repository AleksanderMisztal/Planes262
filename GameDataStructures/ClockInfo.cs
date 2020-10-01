namespace GameDataStructures
{
    public readonly struct ClockInfo
    {
        public readonly int InitialTimeS;
        public readonly int IncrementS;
        public readonly long StartTimestamp;

        public ClockInfo(int initialTimeS, int incrementS, long startTimestamp)
        {
            InitialTimeS = initialTimeS;
            IncrementS = incrementS;
            StartTimestamp = startTimestamp;
        }
    }
}