using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public static class VectorTwoConversion
    {
        public static Vector3Int ToVector3Int(this VectorTwo v) => new Vector3Int(v.X, v.Y, 0);
        public static Vector3 ToVector3(this VectorTwo v) => new Vector3(v.X, v.Y, 0);
    }
}
