using Planes262.Networking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class Messenger : MonoBehaviour
    {
        private static Messenger instance;

        [SerializeField] private InputField input;
        [SerializeField] private Text messagePrefab;
        [SerializeField] private GameObject textParent;

        private List<GameObject> messages = new List<GameObject>();
        private ClientSend sender;

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

        public void SendAMessage()
        {
            string message = UIManager.Username + ": " + input.text;
            input.text = "";

            sender.SendMessage(message);

            Display(message);
        }

        public static void MessageReceived(string message)
        {
            instance.Display(message);
        }

        private void Display(string message)
        {
            Text messageObject = Instantiate(messagePrefab, textParent.transform, true);
            messageObject.text = message;
            messageObject.transform.localScale = new Vector3(.95f, 1, 1);

            messages.Add(messageObject.gameObject);
        }

        public static void ResetMessages()
        {
            foreach (GameObject message in instance.messages) Destroy(message);
            instance.messages = new List<GameObject>();
        }

        public static void SetSender(ClientSend sender)
        {
            instance.sender = sender;
        }
    }
}