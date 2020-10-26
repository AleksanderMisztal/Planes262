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


        public void PlayLocal()
        {
            GameInitializer.LoadBoard("level2", true);
        }

        public void PlayOnline()
        {
            PlayerMeta.name = username.text;
            GameInitializer.LoadBoard("xd", false);
            Client.instance.JoinGame();
        }

        public void EditLevel()
        {
            SceneManager.LoadScene("Editor Menu");
        }
    }
}