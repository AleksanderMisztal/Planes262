using UnityEngine;

namespace GameServer.Utils
{
    public class VectorTwo
    {
        public int X { get; }
        public int Y { get; }

        public int SqrMagnitude => X * X + Y * Y;


        public VectorTwo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public VectorTwo()
        {
            X = 0;
            Y = 0;
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

        public static explicit operator Vector3(VectorTwo v) => new Vector3(v.X, v.Y, 0);
        public static explicit operator UnityEngine.Vector3Int(VectorTwo v) => new UnityEngine.Vector3Int(v.X, v.Y, 0);

        public static VectorTwo operator + (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.X + b.X, a.Y + b.Y);

        public static VectorTwo operator - (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.X - b.X, a.Y - b.Y);


        public static bool operator == (VectorTwo a, VectorTwo b)
            => a.X == b.X && a.Y == b.Y;

        public static bool operator != (VectorTwo a, VectorTwo b)
            => a.X != b.X || a.Y != b.Y;
    }
}
