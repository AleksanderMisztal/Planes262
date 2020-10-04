using System.Collections.Generic;
using System.Linq;

namespace GameDataStructures.Packets
{
    public static class Serializer
    {
        public static byte[] Deserialize(string data) =>
            data.Split(',').Select(byte.Parse).ToArray();

        public static string Serialize(IEnumerable<byte> bytes) =>
            string.Join(",", bytes);
    }
}
