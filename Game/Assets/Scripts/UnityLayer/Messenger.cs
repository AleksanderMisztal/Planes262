using System;
using System.Collections.Generic;
using Planes262.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Planes262.UnityLayer
{
    public class Messenger : MonoBehaviour
    {
        [SerializeField] private InputField input;
        [SerializeField] private Text messagePrefab;
        [SerializeField] private GameObject textParent;

        private List<GameObject> messages = new List<GameObject>();
        private string username;

        public void SetUsername(string aUsername)
        {
            username = aUsername;
        }

        public void SendAMessage()
        {
            string message = username + ": " + input.text;
            input.text = "";
            Client.instance?.SendAMessage(message);
            Display(message);
        }

        public void MessageReceived(string message)
        {
            Display(message);
        }

        private void Display(string message)
        {
            Text messageObject = Instantiate(messagePrefab, textParent.transform, true);
            messageObject.text = message;
            messageObject.transform.localScale = new Vector3(.95f, 1, 1);

            messages.Add(messageObject.gameObject);
        }

        public void ResetMessages()
        {
            foreach (GameObject message in messages) Destroy(message);
            messages = new List<GameObject>();
        }
    }
}