using System.Collections.Generic;
using System.Text;

namespace GameDataStructures.Packets
{
    public class Merger
    {
        private readonly StringBuilder sb = new StringBuilder();

        public Merger Write(IWriteable w) => Write(w.Data);

        public Merger Write(object w) => Write(w.ToString());

        private Merger Write(string s)
        {
            sb.Append('(').Append(s).Append(')');
            return this;
        }

        public string Data => sb.ToString();
        
        public static List<string> Split(string data)
        {
            List<string> args = new List<string>();

            for (int i = 0, diff = 0, open = 0; i < data.Length; i++)
            {
                if (data[i] == '(') diff++;
                if (data[i] == ')') diff--;
                if (diff != 0) continue;
                args.Add(data.Substring(open + 1, i - open - 1));
                open = i + 1;
            }
            return args;
        }
    }
}
