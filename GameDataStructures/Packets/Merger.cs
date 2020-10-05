using System.Collections.Generic;
using System.Linq;

namespace GameDataStructures.Packets
{
    public static class Merger
    {
        public static List<string> Split(string data) =>
            data.Split(';').ToList();

        public static string Join(IEnumerable<string> objects) =>
            string.Join(";", objects);
    }
}
