using System.IO;
using GameDataStructures.Dtos;
using UnityEngine;

namespace Planes262.LevelEditor
{
    public static class Saver
    {
        private const string fileExtension = ".txt";
        private static readonly string savePath;

        static Saver()
        {
            savePath = Application.dataPath + "/Saves/";
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
        }

        public static void Save(string fileName, LevelDto obj)
        {
            string data = JsonUtility.ToJson(obj);
            string saveName = savePath + fileName + fileExtension;
            File.WriteAllText(saveName, data);
        }

        public static LevelDto Read(string fileName)
        {
            string saveName = savePath + fileName + fileExtension;
            string data = File.ReadAllText(saveName);
            LevelDto read = JsonUtility.FromJson<LevelDto>(data);
            return read;
        }
    }
}