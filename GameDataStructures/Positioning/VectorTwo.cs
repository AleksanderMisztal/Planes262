using System;

namespace GameDataStructures.Positioning
{
    [Serializable]
    public readonly struct VectorTwo
    {
        public readonly int x;
        public readonly int y;

        public VectorTwo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override int GetHashCode() => 1000 * x + y;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            VectorTwo v = (VectorTwo)obj;
            return (x == v.x) && (y == v.y);
        }

        public override string ToString() => $"({x}, {y})";

        public static VectorTwo operator + (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.x + b.x, a.y + b.y);

        public static VectorTwo operator - (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.x - b.x, a.y - b.y);


        public static bool operator == (VectorTwo a, VectorTwo b)
            => a.x == b.x && a.y == b.y;

        public static bool operator != (VectorTwo a, VectorTwo b)
            => a.x != b.x || a.y != b.y;
    }
}
