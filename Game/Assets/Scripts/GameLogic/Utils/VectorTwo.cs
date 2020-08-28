namespace Planes262.GameLogic.Utils
{
    public class VectorTwo
    {
        public int X { get; }
        public int Y { get; }


        public VectorTwo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return 1000 * (X + 100) + Y + 100;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
            VectorTwo v = (VectorTwo)obj;
            return (X == v.X) && (Y == v.Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static VectorTwo operator + (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.X + b.X, a.Y + b.Y);

        public static VectorTwo operator - (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.X - b.X, a.Y - b.Y);


        public static bool operator == (VectorTwo a, VectorTwo b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator != (VectorTwo a, VectorTwo b)
        {
            return !(a == b);
        }
    }
}
