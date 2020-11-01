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
        [SerializeField] private Transform localLevels;
        [SerializeField] private Transform onlineLevels;
        [SerializeField] private Button loadablePrefab;

        private void Start()
        {
            CreateButtons(PersistState.gameTypes, onlineLevels);
            Client.instance.serverEvents.OnWelcome += gameTypes => CreateButtons(gameTypes, onlineLevels);
            DisplayLoadableLevels();
            loadMenu.SetActive(false);
        }

        private void CreateButtons(IEnumerable<string> gameTypes, Transform parent)
        {
            PersistState.gameTypes = gameTypes;
            foreach (string gameType in gameTypes)
            {
                Button button = Instantiate(loadablePrefab, parent);
                Text text = button.transform.GetChild(0).GetComponent<Text>();
                text.text = gameType;
                button.onClick.AddListener(() => PlayOnline(gameType));
            }
        }

        private void DisplayLoadableLevels()
        {
            IEnumerable<string> levels = GetLevels();
            CreateButtons(levels, localLevels);
        }
        
        private static IEnumerable<string> GetLevels()
        {
            string path = Application.dataPath + "/Saves/";
            try
            {
                return Directory.GetDirectories(path).Select(d => d.Split('/').Last());
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

        public void PlayOnline(string gameType)
        {
            PlayerMeta.name = username.text;
            GameInitializer.LoadBoard("level1", false);
            Client.instance.JoinGame(gameType);
        }

        public void EditLevel()
        {
            SceneManager.LoadScene("Editor Menu");
        }
    }
}