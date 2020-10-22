using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Planes262.UnityLayer.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private InputField username;

        [SerializeField] private GameObject particles;
        [SerializeField] private GameObject mainBackground;
        
        [SerializeField] private GameObject waitingText;

        public void PlayLocal()
        {
            PersistentState.isLocal = true;
            SceneManager.LoadScene("Board");
        }

        public void PlayOnline()
        {
            PersistentState.isLocal = false;
            PlayerMeta.name = username.text;
            
            username.gameObject.SetActive(false);
            mainBackground.SetActive(false);
            
            particles.SetActive(true);
            waitingText.SetActive(true);
        }
    }
}