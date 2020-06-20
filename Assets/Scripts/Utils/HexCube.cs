using UnityEngine;

namespace Scripts.Utils
{
    public class HexCube
    {
        private readonly int q;
        private readonly int r;
        private readonly int s;


        public HexCube(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public HexCube(Vector3Int v)
        {
            q = v.x;
            r = v.y;
            s = v.z;
        }


        public Vector3Int ToVector()
        {
            return new Vector3Int(q, r, s);
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
