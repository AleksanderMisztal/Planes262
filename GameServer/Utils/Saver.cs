using System.Text.Json;
using GameDataStructures.Dtos;

namespace GameServer.Utils
{
    public static class Json
    {
        public static LevelDto Read(string obj)
        {
            return JsonSerializer.Deserialize<LevelDto>(obj);
        }
    }
}