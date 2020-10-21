using System;
using System.Collections;
using GameDataStructures;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private InputField username;
        [SerializeField] private GameObject playButton;

        [SerializeField] private GameObject particles;
        [SerializeField] private GameObject mainBackground;
        
        [SerializeField] private GameObject waitingText;
        
        [SerializeField] private GameObject boardCamera;
        [SerializeField] private GameObject gameUi;
        
        [SerializeField] private GameObject gameEnded;
        [SerializeField] private Text resultText;

        public event Action GameJoined;

        private void Start()
        {
            waitingText.SetActive(false);
            
            gameUi.SetActive(false);
            boardCamera.SetActive(false);
            
            gameEnded.SetActive(false);

            Camera.main.rect = new Rect(0, 0, 1, 1);
        }

        public void ActivateMainMenu()
        {
            Debug.Log("Activating main");
            username.gameObject.SetActive(true);
            playButton.SetActive(true);
        }

        public void JoinGame()
        {
            PlayerMeta.Name = username.text;
            GameJoined?.Invoke();
        }

        public void TransitionIntoGame(Board boardDims)
        {
            particles.SetActive(false);
            mainBackground.SetActive(false);
            
            waitingText.SetActive(false);

            gameUi.SetActive(true);
            boardCamera.SetActive(true);

            boardCamera.GetComponent<BoardCamera>().Initialize(boardDims);
        }

        public void EndGame(string message, float delay)
        {
            StartCoroutine(Co_EndGame(message, delay));
        }

        private IEnumerator Co_EndGame(string message, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            mainBackground.SetActive(true);
            particles.SetActive(true);
            
            gameEnded.SetActive(true);
            resultText.text = message;
        }
    }
}
