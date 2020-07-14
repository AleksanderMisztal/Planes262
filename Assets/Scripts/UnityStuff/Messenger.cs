using Scripts.Networking;
using Scripts.UnityStuff;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messenger : MonoBehaviour
{
    private class MessageInfo
    {
        public string Sender { get; }
        public string Text { get; }

        public MessageInfo(string sender, string text)
        {
            Sender = sender;
            Text = text;
        }
    }

    private static Messenger instance;

    [SerializeField]
    private InputField input;

    [SerializeField]
    private Text messagePrefab;

    [SerializeField]
    private GameObject textParent;

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
        string message = input.text;
        input.text = "";

        ClientSend.SendAMessage(message);

        Text messageObject = Instantiate(messagePrefab);
        messageObject.text = UIManager.Username + ": " + message;
        messageObject.transform.SetParent(textParent.transform);

        messages.Add(messageObject.gameObject);
    }

    public static void MessageReceived(string message)
    {
        Text newMessage = Instantiate(instance.messagePrefab);
        newMessage.text = UIManager.Username + ": " + message;

        instance.messages.Add(newMessage.gameObject);
    }

    public static void ResetMessages()
    {
        foreach (var message in instance.messages)
        {
            Destroy(message);
        }
        instance.messages = new List<GameObject>();
    }
}
