using Planes262.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Planes262.UnityLayer.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private InputField username;
        [SerializeField] private GameObject playLocalButton;
        [SerializeField] private GameObject playOnlineButton;
        [SerializeField] private GameObject cancelButton;
        [SerializeField] private GameObject loadingIcon;


        public void PlayLocal()
        {
            PersistState.isLocal = true;
            SceneManager.LoadScene("Board");
        }

        public void PlayOnline()
        {
            PersistState.isLocal = false;
            PlayerMeta.name = username.text;
            
            SetIsJoining(true);
            SceneManager.LoadScene("Board");

            Client.instance.JoinGame();
        }

        public void Cancel()
        {
            SetIsJoining(false);
            // TODO: Client.instance.CancelJoin();
        }

        private void SetIsJoining(bool joining)
        {
            playLocalButton.SetActive(!joining);
            playOnlineButton.SetActive(!joining);
            
            username.gameObject.SetActive(!joining);
            
            cancelButton.SetActive(joining);
            loadingIcon.SetActive(joining);
        }
    }
}