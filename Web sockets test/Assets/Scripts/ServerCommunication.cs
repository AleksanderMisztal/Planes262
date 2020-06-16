using UnityEngine;

public class ServerCommunication : MonoBehaviour
{
    private string hostIP;
    private int port = 5000;
    private bool useLocalhost = true;
    private string Host => useLocalhost ? "localhost" : hostIP;
    private string server;

    private WsClient client;
    public LobbyMessaging Lobby { get; private set; }


    private void Awake()
    {
        server = "ws://" + Host + ":" + port;
        client = new WsClient(server);

        Lobby = new LobbyMessaging(this);
    }

    private void Update()
    {
        var cqueue = client.ReceiveQueue;
        while (cqueue.TryDequeue(out string msg))
        {
            HandleMessage(msg);
        }
    }


    private void HandleMessage(string msg)
    {
        Debug.Log("Server: " + msg);
        var message = JsonUtility.FromJson<MessageModel>(msg);
        switch (message.method)
        {
            case LobbyMessaging.Register:
                Lobby.OnConnectedToServer?.Invoke();
                break;
            case LobbyMessaging.Echo:
                Lobby.OnEchoMessage?.Invoke(JsonUtility.FromJson<EchoMessageModel>(message.message));
                break;
            default:
                Debug.LogError("Unknown type of method: " + message.method);
                break;
        }
    }

    public async void ConnectToServer()
    {
        await client.Connect();
    }

    public void SendRequest(string message)
    {
        client.Send(message);
    }
}