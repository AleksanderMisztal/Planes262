using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Planes262.LevelEditor
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private InputField background;
        [SerializeField] private InputField levelName;
        [SerializeField] private GameObject loadableLevels;
        [SerializeField] private Button loadablePrefab;
        

        public void DisplayLoadableLevels()
        {
            IEnumerable<string> levels = GetDirs();
            foreach (string level in levels)
            {
                string currentLevel = level.Split('/').Last();
                Button button = Instantiate(loadablePrefab, loadableLevels.transform);
                Text text = button.transform.GetChild(0).GetComponent<Text>();
                text.text = currentLevel;
                button.onClick.AddListener(() => LoadLevel(text.text));
            }
        }
        
        private static IEnumerable<string> GetDirs()
        {
            string path = Application.dataPath + "/Saves/";
            try
            {
                return Directory.GetDirectories(path);
            }
            catch
            {
                return new string[0];
            }
        }

        private void LoadLevel(string level)
        {
            LevelConfig.name = level;
            LevelConfig.isLoaded = true;
            TransitionToEditing();
        }

        public void TransitionToNewLevel()
        {
            LevelConfig.name = levelName.text;
            LevelConfig.background = background.text;
            TransitionToEditing();
        }

        private void TransitionToEditing()
        {
            string path = Application.dataPath + "/Saves/" + levelName.text;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            else Debug.Log("Will override");
            SceneManager.LoadScene("Level Editor");
        }

        public void BackToMain()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}