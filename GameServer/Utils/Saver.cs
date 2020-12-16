using GameDataStructures.Dtos;
using Newtonsoft.Json;

namespace GameServer.Utils
{
    public static class Json
    {
        public static LevelDto Read(string obj)
        {
            return JsonConvert.DeserializeObject<LevelDto>(obj);
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}