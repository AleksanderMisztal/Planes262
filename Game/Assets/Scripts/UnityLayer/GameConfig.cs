using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameDataStructures.Dtos;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public static class GameConfig
    {
        static GameConfig()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
        }
        
        public static IEnumerable<string> onlineLevels = new List<string>();
        public static IEnumerable<string> LocalLevels => levelDtos.Keys;
        private static readonly Dictionary<string, LevelDto> levelDtos = new Dictionary<string, LevelDto>();

        private const string fileExtension = ".txt";
        private static readonly string savePath = Application.dataPath + "/Saves/";
        private const string level0 = "{\"background\":\"board\",\"board\":{\"xSize\":18,\"ySize\":10},\"cameraDto\":{\"xOffset\":13.190206527709961,\"yOffset\":8.800106048583985,\"ortoSize\":12.0},\"troopDtos\":[{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":10,\"y\":3},\"orientation\":3,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":3},\"orientation\":3,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":4},\"orientation\":1,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":12,\"y\":4},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":4},\"orientation\":3,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":14,\"y\":4},\"orientation\":4,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":5},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":8,\"y\":5},\"orientation\":1,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":9,\"y\":5},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":10,\"y\":5},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":5},\"orientation\":4,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":6},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":8,\"y\":6},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":10,\"y\":6},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":6},\"orientation\":4,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":7},\"orientation\":0,\"movePoints\":0,\"health\":0}]}";
        public static LevelDto GetLevel(string levelName) => levelDtos.TryGetValue(levelName, out LevelDto dto) ? dto : null;
        private static LevelDto Level0() => JsonUtility.FromJson<LevelDto>(level0);
        
        
        public static void LoadLocalLevels()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            IEnumerable<string> localLevels = Directory.GetFiles(savePath)
                .Where(p => p.EndsWith(".txt"))
                .Select(Path.GetFileNameWithoutExtension);
            foreach (string levelName in localLevels)
                levelDtos[levelName] = Read(levelName);
#endif
            levelDtos["level0"] = Level0();
        }

        public static void Save(string levelName, LevelDto dto)
        {
            levelDtos[levelName] = dto;
            string data = JsonUtility.ToJson(dto);
            Debug.Log(data);
            string saveName = savePath + levelName + fileExtension;
            File.WriteAllText(saveName, data);
        }

        private static LevelDto Read(string fileName)
        {
            string saveName = savePath + fileName + fileExtension;
            string data = File.ReadAllText(saveName);
            LevelDto read = JsonUtility.FromJson<LevelDto>(data);
            return read;
        }
    }
}