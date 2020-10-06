using System.Collections.Generic;
using GameDataStructures.Packets;

namespace GameDataStructures
{
    public struct ClockInfo : IWriteable, IReadable
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


        public IReadable Read(string s)
        {
            List<string> args = Merger.Split(s);

            InitialTimeS = int.Parse(args[0]);
            IncrementS = int.Parse(args[1]);
            StartTimestamp = long.Parse(args[2]);

            return this;
        }
        
        public string Data => new Merger().Write(InitialTimeS).Write(IncrementS).Write(StartTimestamp).Data;
    }
}