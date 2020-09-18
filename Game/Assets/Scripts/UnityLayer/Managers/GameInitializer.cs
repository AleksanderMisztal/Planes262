using System.Threading.Tasks;
using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        private UIManager uiManager;
        private Messenger messenger;
        private GameManager gameManager;

        private async void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            gameManager = FindObjectOfType<GameManager>();
            
            await InitializeNetworking();
        }

        private async Task InitializeNetworking()
        {
            LocalJudge judge = new LocalJudge(uiManager, gameManager);
            ServerJudge serverJudge = new ServerJudge(messenger, uiManager, gameManager);
            
            ClientHandle clientHandle = new ClientHandle(serverJudge);
            CsWebSocketClient wsClient = new CsWebSocketClient(clientHandle);
            Client client = new Client(wsClient);
            
            gameManager.MoveAttempted += (sender, args) => client.MoveTroop(args.Position, args.Direction);
            uiManager.GameJoined += (sender, username) => client.JoinGame(username);
            messenger.MessageSent += (sender, message) => client.SendMessage(message);
            
            await wsClient.InitializeConnection();
            await wsClient.BeginListenAsync();
        }
    }
}