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
            var message = UIManager.Username + ": " + input.text;
            input.text = "";

            ClientSend.SendMessage(message);

            Display(message);
        }

        public static void MessageReceived(string message)
        {
            instance.Display(message);
        }

        private void Display(string message)
        {
            var messageObject = Instantiate(messagePrefab, textParent.transform, true);
            messageObject.text = message;
            messageObject.transform.localScale = new Vector3(.95f, 1, 1);

            messages.Add(messageObject.gameObject);
        }

        public static void ResetMessages()
        {
            foreach (var message in instance.messages) Destroy(message);
            instance.messages = new List<GameObject>();
        }
    }
}