using System.Collections.Generic;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;

namespace GameDataStructures
{
    public class Board : IReadable, IWriteable
    {
        public readonly int xSize;
        public readonly int ySize;

        public VectorTwo Center { get; }

        public Board(int xSize, int ySize)
        {
            this.xSize = xSize;
            this.ySize = ySize;

            Center = new VectorTwo(xSize / 2, ySize / 2);
        }
        
        public bool IsInside(VectorTwo p)
        {
            return p.x >= 0 && p.x < xSize && p.y >= 0 && p.y < ySize;
        }

        
        public Board() { }

        public IReadable Read(string s)
        {
            List<string> args = Merger.Split(s);

            int newX = int.Parse(args[0]);
            int newY = int.Parse(args[1]);

            return new Board(newX, newY);
        }

        public string Data => new Merger().Write(xSize).Write(ySize).Data;
    }
}
