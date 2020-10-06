using System.Collections.Generic;
using GameDataStructures.Packets;

namespace GameDataStructures
{
    public struct TimeInfo : IReadable, IWriteable
    {
        public int RedTimeMs { get; private set; }
        public int BlueTimeMs { get; private set; }
        public long ChangeTimeMs { get; private set; }

        public TimeInfo(int redTimeMs, int blueTimeMs, long changeTimeMs)
        {
            RedTimeMs = redTimeMs;
            BlueTimeMs = blueTimeMs;
            ChangeTimeMs = changeTimeMs;
        }

        public IReadable Read(string s)
        {
            List<string> props = Merger.Split(s);
            
            RedTimeMs = int.Parse(props[0]);
            BlueTimeMs = int.Parse(props[1]);
            ChangeTimeMs = long.Parse(props[2]);

            return this;
        }

        public string Data => new Merger().Write(RedTimeMs).Write(BlueTimeMs).Write(ChangeTimeMs).Data;
    }
}