using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Messages.Server;
using Planes262.Networking;
using Planes262.UnityLayer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planes262.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private InputField username;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject localMenu;
        [SerializeField] private GameObject onlineMenu;
        [SerializeField] private GameObject settings;
        [SerializeField] private Transform localLevelsScroll;
        [SerializeField] private Transform onlineLevelsScroll;
        [SerializeField] private Button loadablePrefab;

        private void Start()
        {
            GameConfig.LoadLocalLevels();
            
            CreateButtons(GameConfig.LocalLevels, localLevelsScroll, PlayLocal);
            CreateButtons(GameConfig.onlineLevels, onlineLevelsScroll, PlayOnline);
            
            Client.instance.serverEvents.OnWelcome += gameTypes => {
                GameConfig.onlineLevels = gameTypes;
                CreateButtons(gameTypes, onlineLevelsScroll, PlayOnline);
            };
            localMenu.SetActive(false);
            onlineMenu.SetActive(false);
            if (string.IsNullOrEmpty(PlayerMeta.name)) ShowSettings();
            else username.text = PlayerMeta.name;
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

        public void ShowSettings()
        {
            mainMenu.SetActive(false);
            settings.SetActive(true);
        }
        
        public void PlayLocal()
        {
            mainMenu.SetActive(false);
            localMenu.SetActive(true);
        }
        
        public void PlayOnline()
        {
            mainMenu.SetActive(false);
            onlineMenu.SetActive(true);
        }
        
        private void PlayLocal(string level)
        {
            LevelDto dto = GameConfig.GetLevel(level);
            GameInitializer.LoadGame(new GameJoinedMessage("p2", PlayerSide.Blue, dto, new ClockInfo()), true);
        }

        public void PlayOnline(string gameType)
        {
            PlayerMeta.name = username.text;
            Client.instance.JoinGame(gameType);
        }

        public void EditLevel()
        {
            SceneManager.LoadScene("Editor Menu");
        }
    }
}