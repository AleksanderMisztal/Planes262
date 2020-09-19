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
        [SerializeField] private GameObject mainMenu;
        
        [SerializeField] private GameObject particles;
        [SerializeField] private GameObject mainBackground;
        
        [SerializeField] private GameObject waitingText;
        
        [SerializeField] private GameObject boardCamera;
        [SerializeField] private GameObject board;
        [SerializeField] private GameObject gameUi;
        
        [SerializeField] private GameObject gameEnded;
        [SerializeField] private Text resultText;

        public event EventHandler<string> GameJoined;
        public event Action LocalPlayed;
        private void Start()
        {
            mainMenu.SetActive(false);
            
            waitingText.SetActive(false);
            
            gameUi.SetActive(false);
            board.SetActive(false);
            boardCamera.SetActive(false);
            
            gameEnded.SetActive(false);

            Camera.main.rect = new Rect(0, 0, 1, 1);
        }

        public void ActivateMainMenu()
        {
            mainMenu.SetActive(true);
        }

        public void JoinGame()
        {
            GameJoined?.Invoke(this, username.text);
            mainMenu.SetActive(false);
            
            waitingText.SetActive(true);
        }

        public void PlayLocal()
        {
            LocalPlayed?.Invoke();
            mainMenu.SetActive(false);
            TransitionIntoGame(Board.Standard);
        }

        public void TransitionIntoGame(Board boardDims)
        {
            particles.SetActive(false);
            mainBackground.SetActive(false);
            
            waitingText.SetActive(false);

            gameUi.SetActive(true);
            boardCamera.SetActive(true);
            board.SetActive(true);

            boardCamera.GetComponent<BoardCamera>().Initialize(boardDims);
        }

        public void EndGame(string message, float delay)
        {
            StartCoroutine(Co_EndGame(message, delay));
        }

        private IEnumerator Co_EndGame(string message, float delay)
        {
            yield return new WaitForSeconds(delay);

            board.SetActive(false);
            
            mainBackground.SetActive(true);
            particles.SetActive(true);
            
            gameEnded.SetActive(true);
            resultText.text = message;
        }

        public void BackToMainMenu()
        {
            gameEnded.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
