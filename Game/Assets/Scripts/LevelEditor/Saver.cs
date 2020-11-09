using System.IO;
using UnityEngine;

namespace Planes262.LevelEditor
{
    public static class Saver
    {
        private const string fileExtension = ".txt";
        private static readonly string savePath = Application.dataPath + "/Saves/";

        static Saver()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
        }

        public static void Save(string fileName, object obj)
        {
            string data = JsonUtility.ToJson(obj);
            string saveName = savePath + fileName + fileExtension;
            File.WriteAllText(saveName, data);
        }

        public static T Read<T>(string fileName)
        {
            string saveName = savePath + fileName + fileExtension;
            string data = File.ReadAllText(saveName);
            T read = JsonUtility.FromJson<T>(data);
            return read;
        }
    }
}