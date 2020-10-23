using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class GameEndedScreen : MonoBehaviour
    {
        [SerializeField] private Text resultText;

        private void Start()
        {
            resultText.text = PersistState.gameEndedMessage;
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}