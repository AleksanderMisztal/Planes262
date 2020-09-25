namespace GameDataStructures
{
    public readonly struct TimeInfo
    {
        public readonly int RedTimeMs;
        public readonly int BlueTimeMs;
        public readonly long ChangeTimeMs;

        public TimeInfo(int redTimeMs, int blueTimeMs, long changeTimeMs)
        {
            RedTimeMs = redTimeMs;
            BlueTimeMs = blueTimeMs;
            ChangeTimeMs = changeTimeMs;
        }
    }
}