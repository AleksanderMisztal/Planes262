using System;
using System.Collections.Generic;
using GameDataStructures.Packets;

namespace GameDataStructures.Positioning
{
    [Serializable]
    public readonly struct VectorTwo : IWriteable, IReadable
    {
        public readonly int x;
        public readonly int y;

        public VectorTwo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override int GetHashCode()
        {
            return 1000 * x + y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            VectorTwo v = (VectorTwo)obj;
            return (x == v.x) && (y == v.y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static VectorTwo operator - (VectorTwo a, VectorTwo b)
            => new VectorTwo(a.x - b.x, a.y - b.y);


        public static bool operator == (VectorTwo a, VectorTwo b)
            => a.x == b.x && a.y == b.y;

        public static bool operator != (VectorTwo a, VectorTwo b)
            => a.x != b.x || a.y != b.y;
        
        
        public IReadable Read(string s)
        {
            List<string> props = Merger.Split(s);
            
            int newX = int.Parse(props[0]);
            int newY = int.Parse(props[1]);
            
            return new VectorTwo(newX, newY);
        }
        
        public string Data => new Merger().Write(x).Write(y).Data;
    }
}
