using Scripts.GameLogic;
using Scripts.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UnityStuff
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [SerializeField] private InputField username;
        [SerializeField] private Text oponentName;
        [SerializeField] private Text resultText;

        private GameObject mainMenu;
        private GameObject board;
        private GameObject particles;
        private GameObject waitingText;
        private GameObject gameUI;
        private GameObject gameEnded;

        private int oponentId = -1;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }

        private void Start()
        {
            board = GameObject.FindWithTag("Board");
            particles = GameObject.FindWithTag("Particles");
            gameUI = GameObject.FindWithTag("Game UI");
            gameEnded = GameObject.FindWithTag("Game Ended");
            mainMenu = GameObject.FindWithTag("Main Menu");
            waitingText = GameObject.FindWithTag("Waiting");

            board.SetActive(false);
            waitingText.SetActive(false);
            gameUI.SetActive(false);
            gameEnded.SetActive(false);
            mainMenu.SetActive(false);
        }

        public void JoinLobby()
        {
            ClientSend.JoinLobby(username.text);
            ClientSend.JoinGame(oponentId);

            mainMenu.SetActive(false);
            board.SetActive(true);
            waitingText.SetActive(true);
        }

        public void BackToMainMenu()
        {
            gameEnded.SetActive(false);
            gameUI.SetActive(false);
            mainMenu.SetActive(true);
        }

        public static void OnConnected()
        {
            instance.mainMenu.SetActive(true);
            TransitionController.EndTransition();
        }

        public static void StartGame(PlayerId side, string oponentName)
        {
            TransitionController.StartTransition();

            instance.waitingText.SetActive(false);
            instance.particles.SetActive(false);

            instance.gameUI.SetActive(true);
            instance.oponentName.text = oponentName;

            TransitionController.EndTransition();
        }

        public static void OpponentDisconnected()
        { 
            string message = "Opponent has disconnected :(";
            instance.EndGame(message);
        }

        public static void EndGame(int blueScore, int redScore)
        {
            string message = $"Final score: red: {redScore}, blue: {blueScore}";
            instance.EndGame(message);
        }

        private void EndGame(string message)
        {
            GameController.instance.EndGame();
            board.SetActive(false);
            gameEnded.SetActive(true);
            particles.SetActive(true);
            TileManager.DeactivateTiles();
            resultText.text = message;
        }
    }
}
