using System.Collections;
using GameDataStructures;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject boardCamera;
        [SerializeField] private GameObject gameUi;
        
        [SerializeField] private GameObject gameEnded;
        [SerializeField] private Text resultText;

        private void Start()
        {
            gameEnded.SetActive(false);
            gameUi.SetActive(true);
            boardCamera.SetActive(true);

            boardCamera.GetComponent<BoardCamera>().Initialize(PersistentState.board);
        }

        public void EndGame(string message, float delay)
        {
            StartCoroutine(Co_EndGame(message, delay));
        }

        private IEnumerator Co_EndGame(string message, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            gameEnded.SetActive(true);
            resultText.text = message;
        }
    }
}
