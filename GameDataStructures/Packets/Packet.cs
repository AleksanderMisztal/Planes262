using System.Collections.Generic;

namespace GameDataStructures.Packets
{
    public class Packet
    {
        private readonly List<string> objects;
        private readonly Merger merger = new Merger();
        private int index;

        public string Data => merger.Data;

        public Packet(string serialized)
        {
            objects = Merger.Split(serialized);
        }

        public Packet(ServerPackets id)
        {
            objects = new List<string>();
            Write((int)id);
        }
        
        public Packet(ClientPackets id)
        {
            objects = new List<string>();
            Write((int)id);
        }

        public void Write(object obj)
        {
            Write(obj.ToString());
        }

        public void Write(IWriteable writeable)
        {
            Write(writeable.Data);
        }

        private void Write(string obj)
        {
            objects.Add(obj);
            merger.Write(obj);
        }

        public T Read<T>() where T : IReadable, new()
        {
            return (T)new T().Read(objects[index++]);
        }

        public List<T> ReadList<T>() where T : IReadable, new()
        {
            int count = ReadInt();
            List<T> ts = new List<T>();
            for (int i = 0; i < count; i++) ts.Add(Read<T>());
            return ts;
        }

        public int ReadInt() => int.Parse(objects[index++]);

        public string ReadString() => objects[index++];
    }
}