using System.Collections.Generic;
using System.Linq;

namespace GameDataStructures.Positioning
{
    public class HexOffset
    {
        private static readonly VectorTwo[] evenSteps = {
            new VectorTwo(1, -1),
            new VectorTwo(1, 0),
            new VectorTwo(0, 1),
            new VectorTwo(-1, 0),
            new VectorTwo(-1, -1),
            new VectorTwo(0, -1),
        };
        private static readonly VectorTwo[] oddSteps = {
            new VectorTwo(1, 0),
            new VectorTwo(1, 1),
            new VectorTwo(0, 1),
            new VectorTwo(-1, 1),
            new VectorTwo(-1, 0),
            new VectorTwo(0, -1),
        };

        private readonly int x;
        private readonly int y;

        private HexOffset(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public HexOffset(VectorTwo v)
        {
            x = v.x;
            y = v.y;
        }

        public HexOffset GetAdjacentHex(int direction)
        {
            direction %= 6;
            while (direction < 0) direction += 6;
            VectorTwo[] steps = (x & 1) == 1 ? oddSteps : evenSteps;
            VectorTwo step = steps[direction];
            return new HexOffset(x + step.x, y + step.y);
        }

        public IEnumerable<HexOffset> GetNeighbors()
        {
            VectorTwo[] steps = (x & 1) == 1 ? oddSteps : evenSteps;
            return steps.Select(s => new HexOffset(x + s.x, y + s.y));
        }

        public VectorTwo ToVector()
        {
            return new VectorTwo(x, y);
        }

        public override string ToString()
        {
            return "Offset(" + x + ", " + y + ")";
        }
        
        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType()) return false;
            HexOffset offset = (HexOffset)obj;
            return (x == offset.x) && (y == offset.y);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }
}
