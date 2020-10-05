using GameDataStructures.Packets;

namespace GameDataStructures.Positioning
{
    public struct VectorTwo : IWriteable, IReadable
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public VectorTwo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return 1000 * X + Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            VectorTwo v = (VectorTwo)obj;
            return (X == v.X) && (Y == v.Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static VectorTwo operator - (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.X - b.X, a.Y - b.Y);


        public static bool operator == (VectorTwo a, VectorTwo b)
            => a.X == b.X && a.Y == b.Y;

        public static bool operator != (VectorTwo a, VectorTwo b)
            => a.X != b.X || a.Y != b.Y;

        public string Data => $"{X},{Y}";
        
        public IReadable Read(string s)
        {
            string[] props = s.Split(',');
            
            X = int.Parse(props[0]);
            Y = int.Parse(props[1]);
            
            return this;
        }
    }
}
