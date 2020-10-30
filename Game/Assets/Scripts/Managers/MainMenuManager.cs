using System.Collections.Generic;
using System.IO;
using System.Linq;
using Planes262.Networking;
using Planes262.UnityLayer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Planes262.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private InputField username;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject loadMenu;
        [SerializeField] private GameObject loadableLevels;
        [SerializeField] private Button loadablePrefab;

        private void Start()
        {
            DisplayLoadableLevels();
            loadMenu.SetActive(false);
        }

        private void DisplayLoadableLevels()
        {
            IEnumerable<string> levels = GetDirs();
            foreach (string level in levels)
            {
                string currentLevel = level.Split('/').Last();
                Button button = Instantiate(loadablePrefab, loadableLevels.transform);
                Text text = button.transform.GetChild(0).GetComponent<Text>();
                text.text = currentLevel;
                button.onClick.AddListener(() => PlayLocal(currentLevel));
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

        public void PlayLocal()
        {
            mainMenu.SetActive(false);
            loadMenu.SetActive(true);
        }
        
        private void PlayLocal(string level)
        {
            GameInitializer.LoadBoard(level, true);
        }

        public void PlayOnline()
        {
            PlayerMeta.name = username.text;
            GameInitializer.LoadBoard("level1", false);
            Client.instance.JoinGame();
        }

        public void EditLevel()
        {
            SceneManager.LoadScene("Editor Menu");
        }
    }
}