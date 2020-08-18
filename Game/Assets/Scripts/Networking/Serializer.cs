﻿using System.Linq;

namespace Planes262.Networking
{
    public static class Serializer
    {
        public static byte[] Deserialize(string data) =>
            data.Split(',').Select(s => byte.Parse(s)).ToArray();

        public static string Serialize(byte[] bytes) =>
            string.Join(",", bytes);
    }
}
