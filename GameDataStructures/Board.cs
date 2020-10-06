using System.Collections.Generic;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;

namespace GameDataStructures
{
    public class Board : IReadable, IWriteable
    {
        public int XMax { get; private set; }
        public int YMax { get; private set; }

        public VectorTwo Center { get; }

        public Board(int xMax, int yMax)
        {
            this.XMax = xMax;
            this.YMax = yMax;

            Center = new VectorTwo(xMax / 2, yMax / 2);
        }

        public Board() { }

        public bool IsInside(VectorTwo p)
        {
            return p.X >= 0 && p.X <= XMax && p.Y >= 0 && p.Y <= YMax;
        }

        public static readonly Board Standard = new Board(20, 12);
        public static readonly Board Test = new Board(12, 7);


        public IReadable Read(string s)
        {
            List<string> args = Merger.Split(s);

            XMax = int.Parse(args[0]);
            YMax = int.Parse(args[1]);

            return this;
        }

        public string Data => new Merger().Write(XMax).Write(YMax).Data;
    }
}
