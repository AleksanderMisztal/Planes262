using Planes262.Networking;
using Planes262.UnityLayer;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private InputField username;


        public void PlayLocal()
        {
            GameInitializer.LoadBoard("xd", true);
        }

        public void PlayOnline()
        {
            PlayerMeta.name = username.text;
            GameInitializer.LoadBoard("xd", false);
            Client.instance.JoinGame();
        }
    }
}