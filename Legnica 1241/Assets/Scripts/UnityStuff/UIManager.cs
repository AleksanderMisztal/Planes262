using Scripts.GameLogic;
using Scripts.Networking;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UnityStuff
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [SerializeField] private InputField username;
        [SerializeField] private Text oponentName;

        private GameObject board;
        private GameObject sideIcon;

        private int oponentId = -1;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Debug.Log("Client = this...");
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
            board.SetActive(false);
            sideIcon.SetActive(false);
        }

        public void JoinLobby()
        {
            ClientSend.JoinLobby(username.text);
            ClientSend.JoinGame(oponentId);

            GameObject.FindWithTag("Main Menu").SetActive(false);
            board.SetActive(true);
        }

        public static void SetOponentName(string oponentName)
        {
            instance.oponentName.text = oponentName;
        }

        public static void Activate()
        {
            instance.sideIcon.SetActive(true);
        }

        public static void SetSide(PlayerId side)
        {
            Color color = side == PlayerId.Blue ? Color.blue : Color.red;

            instance.sideIcon.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
