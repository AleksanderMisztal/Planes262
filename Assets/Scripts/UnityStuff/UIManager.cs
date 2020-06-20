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
        private GameObject sideIcon;
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
            sideIcon = GameObject.FindWithTag("Side Icon");
            gameUI = GameObject.FindWithTag("Game UI");
            gameEnded = GameObject.FindWithTag("Game Ended");
            mainMenu = GameObject.FindWithTag("Main Menu");

            board.SetActive(false);
            sideIcon.SetActive(false);
            gameUI.SetActive(false);
            gameEnded.SetActive(false);
        }

        public void JoinLobby()
        {
            ClientSend.JoinLobby(username.text);
            ClientSend.JoinGame(oponentId);

            mainMenu.SetActive(false);
            board.SetActive(true);
        }

        public void BackToMainMenu()
        {
            gameEnded.SetActive(false);
            gameUI.SetActive(false);
            mainMenu.SetActive(true);
        }

        public static void SetOponentName(string oponentName)
        {
            instance.gameUI.SetActive(true);
            instance.oponentName.text = oponentName;
        }

        public static void Activate()
        {
            instance.sideIcon.SetActive(true);
        }

        public static void StartGame(PlayerId side)
        {
            Color color = side == PlayerId.Blue ? Color.blue : Color.red;

            instance.sideIcon.GetComponent<SpriteRenderer>().color = color;
        }

        public static void EndGame(int blueScore, int redScore)
        {
            GameController.instance.EndGame();
            instance.board.SetActive(false);
            instance.gameEnded.SetActive(true);
            instance.resultText.text = $"Final score: red: {redScore}, blue: {blueScore}";
        }
    }
}
