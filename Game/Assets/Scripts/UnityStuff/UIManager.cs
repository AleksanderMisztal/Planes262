using Planes262.GameLogic;
using Scripts.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UnityStuff
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [SerializeField] private InputField username;
        [SerializeField] private Text resultText;
        [SerializeField] private Text participantsText;
        [SerializeField] private GameObject waitingText;
        [SerializeField] private GameObject particles;
        [SerializeField] private GameObject gameUi;
        [SerializeField] private GameObject board;

        private GameObject mainMenu;
        private GameObject gameEnded;
        private GameObject background;
        private GameObject boardCamera;

        public static bool GameStarted { get;  private set; }

        public static string Username => instance.username.text;
        public static string OponentName { get; private set; }

        public static PlayerSide Side { get; private set; }


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
            gameEnded = GameObject.FindWithTag("Game Ended");
            mainMenu = GameObject.FindWithTag("Main Menu");
            background = GameObject.FindWithTag("Background");
            boardCamera = GameObject.FindWithTag("Board Camera");

            gameUi.SetActive(false);
            board.SetActive(false);
            waitingText.SetActive(false);
            gameEnded.SetActive(false);
            mainMenu.SetActive(false);
            boardCamera.SetActive(false);

            Camera.main.rect = new Rect(0, 0, 1, 1);
        }

        public static void OnConnected()
        {
            instance.mainMenu.SetActive(true);
        }

        public void JoinLobby()
        {
            ClientSend.JoinGame(username.text);

            mainMenu.SetActive(false);
            board.SetActive(true);
            waitingText.SetActive(true);

        }

        public static void StartTransitionIntoGame(PlayerSide side, string oponentName, Board board)
        {
            OponentName = oponentName;
            Side = side;

            instance.waitingText.SetActive(false);
            instance.particles.SetActive(false);
            instance.gameUi.SetActive(true);
            instance.background.SetActive(false);
            instance.boardCamera.SetActive(true);

            UpdateScoreDisplay(0, 0);

            TileManager.CreateBoard(board);
            BoardCamera.Initialize(board);
        }

        public static void EndTransitionIntoGame()
        {
            
        }

        public static void UpdateScoreDisplay(int redScore, int blueScore)
        {
            instance.participantsText.text = Side == PlayerSide.Red ?
                $"{OponentName} {blueScore} : {redScore} {Username}" :
                $"{Username} {blueScore} : {redScore} {OponentName}";
        }

        public static void OpponentDisconnected()
        { 
            string message = "Opponent has disconnected :(";
            instance.EndGame(message);
        }

        public static void EndGame(int blueScore, int redScore)
        {
            // TODO: Wait for 1-2 seconds

            string message = $"Final score: red: {redScore}, blue: {blueScore}";
            instance.EndGame(message);
        }

        private void EndGame(string message)
        {
            Debug.Log("UI manager ending the game");
            TileManager.DeactivateTiles();

            instance.background.SetActive(true);
            board.SetActive(false);
            gameEnded.SetActive(true);
            particles.SetActive(true);

            resultText.text = message;
        }

        public void BackToMainMenu()
        {
            gameEnded.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
