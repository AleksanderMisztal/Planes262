namespace GameServer.Utils
{
    class HexCube
    {
        private static readonly VectorThree[] steps = {
            new VectorThree(1, -1, 0),
            new VectorThree(1, 0, -1),
            new VectorThree(0, 1, -1),
            new VectorThree(0, -1, 1),
            new VectorThree(-1, 1, 0),
            new VectorThree(-1, 0, 1)
        };

        private readonly int q;
        private readonly int r;
        private readonly int s;


        public HexCube(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public HexCube(VectorThree v)
        {
            this.q = v.X;
            this.r = v.Y;
            this.s = v.Z;
        }


        public VectorThree ToVector()
        {
            return new VectorThree(q, r, s);
        }

        public HexOffset ToOffset()
        {
            int y = r;
            int x = q + (y - (y & 1)) / 2;
            return new HexOffset(x, y);
        }

        public override string ToString()
        {
            return "HexCube(" + q + ", " + r + ", " + s + ")";
        }
    }
}
