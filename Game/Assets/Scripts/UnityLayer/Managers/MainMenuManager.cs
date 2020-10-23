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


        private void Start()
        {
            Client.instance.serverEvents.OnGameJoined += (opponentName, side, board, troops, clockInfo) =>
            {
                TransitionManager.opponentName = opponentName;
                TransitionManager.side = side;
                TransitionManager.board = board;
                // TODO: troops?
                TransitionManager.clockInfo = clockInfo;
                TransitionManager.isLocal = false;

                SceneManager.LoadScene("Board");
            };
        }

        public void PlayLocal()
        {
            TransitionManager.isLocal = true;
            SceneManager.LoadScene("Board");
        }

        public void PlayOnline()
        {
            TransitionManager.isLocal = false;
            PlayerMeta.name = username.text;
            
            SetIsJoining(true);
            
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