using System.Collections.Generic;
using GameDataStructures.Packets;

namespace GameDataStructures
{
    public class ScoreInfo : IReadable, IWriteable
    {
        public int Red { get; private set; }
        public int Blue { get; private set; }

        public ScoreInfo(int red, int blue)
        {
            Red = red;
            Blue = blue;
        }
        
        public ScoreInfo() { }

        public IReadable Read(string s)
        {
            List<string> props = Merger.Split(s);

            Red = int.Parse(props[0]);
            Blue = int.Parse(props[1]);

            return this;
        }

        public string Data => new Merger().Write(Red).Write(Blue).Data;
    }
}