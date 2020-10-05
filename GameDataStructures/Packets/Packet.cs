using System.Collections.Generic;

namespace GameDataStructures.Packets
{
    public class Packet
    {
        private readonly List<string> objects;
        private int index;

        public string Data => Merger.Join(objects);

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

        private void Write(string obj)
        {
            objects.Add(obj);
        }

        public void Write(object obj)
        {
            Write(obj.ToString());
        }

        public void Write(IWriteable writeable)
        {
            Write(writeable.Data);
        }

        public T Read<T>() where T : IReadable, new()
        {
            return (T)new T().Read(objects[index++]);
        }

        public T ReadPrim<T>() where T : IReadable, new()
        {
            return (T)new T().Read(objects[index++]);
        }
    }
}