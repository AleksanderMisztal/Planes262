public class BaseMessaging
{
    protected ServerCommunication client;

    public BaseMessaging(ServerCommunication client)
    {
        this.client = client;
    }
}

[System.Serializable]
public class MessageModel
{
    public string method;
    public string message;
}

[System.Serializable]
public class EchoMessageModel
{
    public string text;
}