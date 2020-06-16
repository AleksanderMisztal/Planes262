using UnityEngine;
using UnityEngine.Events;

public class LobbyMessaging : BaseMessaging
{
    public LobbyMessaging(ServerCommunication client) : base(client) { }

    // Register messages
    public const string Register = "register";
    public UnityAction OnConnectedToServer;

    // Echo messages
    public const string Echo = "echo";
    public UnityAction<EchoMessageModel> OnEchoMessage;

    public void EchoMessage(EchoMessageModel request)
    {
        var message = new MessageModel
        {
            method = "echo",
            message = JsonUtility.ToJson(request)
        };
        client.SendRequest(JsonUtility.ToJson(message));
    }
}
