using System;

namespace GameDataStructures
{
    [Serializable]
    public readonly struct TimeInfo
    {
        public int RedTimeMs { get; }
        public int BlueTimeMs { get; }
        public long ChangeTimeMs { get; }

        public TimeInfo(int redTimeMs, int blueTimeMs, long changeTimeMs)
        {
            RedTimeMs = redTimeMs;
            BlueTimeMs = blueTimeMs;
            ChangeTimeMs = changeTimeMs;
        }
    }
}