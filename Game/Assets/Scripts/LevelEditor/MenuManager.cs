using System.Linq;
using Planes262.UnityLayer;
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
            foreach (string level in GameConfig.LocalLevels)
            {
                string currentLevel = level.Split('/').Last();
                Button button = Instantiate(loadablePrefab, loadableLevels.transform);
                Text text = button.transform.GetChild(0).GetComponent<Text>();
                text.text = currentLevel;
                button.onClick.AddListener(() => LoadLevel(text.text));
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
            SceneManager.LoadScene("Level Editor");
        }

        public void BackToMain()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}