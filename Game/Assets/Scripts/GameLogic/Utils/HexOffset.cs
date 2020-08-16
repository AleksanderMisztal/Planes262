using System;
using System.Linq;
using GameServer.Utils;

namespace GameServer.Utils
{
    class HexOffset
    {
        private static readonly VectorTwo[] evenSteps = {
            new VectorTwo(1, 0),
            new VectorTwo(0, 1),
            new VectorTwo(-1, 1),
            new VectorTwo(-1, 0),
            new VectorTwo(-1, -1),
            new VectorTwo(0, -1)
        };
        private static readonly VectorTwo[] oddSteps = {
            new VectorTwo(1, 0),
            new VectorTwo(1, 1),
            new VectorTwo(0, 1),
            new VectorTwo(-1, 0),
            new VectorTwo(0, -1),
            new VectorTwo(1, -1)
        };

        private readonly int x;
        private readonly int y;


        public HexOffset(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public HexOffset(VectorTwo v)
        {
            this.x = v.X;
            this.y = v.Y;
        }


        public HexOffset GetAdjacentHex(int direction)
        {
            direction %= 6;
            while (direction < 0) direction += 6;
            VectorTwo[] steps = (y & 1) == 1 ? oddSteps : evenSteps;
            VectorTwo step = steps[direction % 6];
            return new HexOffset(x + step.X, y + step.Y);
        }

        public HexOffset[] GetNeighbors()
        {
            VectorTwo[] steps = (y & 1) == 1 ? oddSteps : evenSteps;
            return steps.Select(s => new HexOffset(x + s.X, y + s.Y)).ToArray();
        }

        public HexCube ToCube()
        {
            var q = x - (y - (y & 1)) / 2;
            var r = y;
            var s = -q - r;
            return new HexCube(q, r, s);
        }

        public VectorTwo ToVector()
        {
            return new VectorTwo(x, y);
        }

        public override string ToString()
        {
            return "Offset(" + x + ", " + y + ")";
        }
        
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
            HexOffset offset = (HexOffset)obj;
            return (x == offset.x) && (y == offset.y);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }
}
