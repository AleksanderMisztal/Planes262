namespace GameServer.Utils
{
    class VectorThree
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public VectorThree(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public VectorThree Abs()
        {
            return new VectorThree(
                X > 0 ? X : -X,
                Y > 0 ? Y : -Y,
                Z > 0 ? Z : -Z
            );
        }

        public static VectorThree operator +(VectorThree a, VectorThree b)
            => new VectorThree(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static VectorThree operator -(VectorThree a, VectorThree b)
            => new VectorThree(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }
}
