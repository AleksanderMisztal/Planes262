using UnityEngine;

public class UsageExample : MonoBehaviour
{
    // Reference to the servercommunication
    [SerializeField]
    private ServerCommunication communication;

    // Reference to UIForm
    [SerializeField]
    private UIForm uiForm;

    private void Start()
    {
        Debug.Log("Starting example");
        communication.Lobby.OnConnectedToServer += OnConnectedToServer;
        communication.ConnectToServer();
    }

    private void OnConnectedToServer()
    {
        communication.Lobby.OnConnectedToServer -= OnConnectedToServer;

        communication.Lobby.OnEchoMessage += OnReceivedEchoMessage;
        uiForm.OnSendButtonClicked += OnSendForm;
    }

    private void OnSendForm()
    {
        var message = new EchoMessageModel
        {
            text = uiForm.InputFieldText
        };
        communication.Lobby.EchoMessage(message);
        uiForm.ClearInputField();
    }

    private void OnReceivedEchoMessage(EchoMessageModel message)
    {
        //Debug.Log("Echo message received: " + message.text);
        uiForm.ShowServerResponse(message.text);
    }

    private void OnDisable()
    {
        communication.Lobby.OnConnectedToServer -= OnConnectedToServer;
        communication.Lobby.OnEchoMessage -= OnReceivedEchoMessage;

        uiForm.OnSendButtonClicked -= OnSendForm;
    }
}