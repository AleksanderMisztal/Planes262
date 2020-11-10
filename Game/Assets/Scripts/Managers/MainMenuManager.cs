using System;
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
        [SerializeField] private Transform localLevelsScroll;
        [SerializeField] private Transform onlineLevelsScroll;
        [SerializeField] private Button loadablePrefab;

        private void Start()
        {
            string savePath = Application.dataPath + "/Saves/";
            PersistState.localLevels = Directory.GetFiles(savePath).Where(p => p.EndsWith(".txt")).Select(Path.GetFileNameWithoutExtension);
            CreateButtons(PersistState.localLevels, localLevelsScroll, PlayLocal);
            CreateButtons(PersistState.onlineLevels, onlineLevelsScroll, PlayOnline);
            Client.instance.serverEvents.OnWelcome += gameTypes => {
                PersistState.onlineLevels = gameTypes;
                CreateButtons(gameTypes, onlineLevelsScroll, PlayOnline);
            };
            loadMenu.SetActive(false);
        }

        private void CreateButtons(IEnumerable<string> gameTypes, Transform parent, Action<string> onGameSelected)
        {
            foreach (string gameType in gameTypes)
            {
                Button button = Instantiate(loadablePrefab, parent);
                Text text = button.transform.GetChild(0).GetComponent<Text>();
                text.text = gameType;
                button.onClick.AddListener(() => onGameSelected(gameType));
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