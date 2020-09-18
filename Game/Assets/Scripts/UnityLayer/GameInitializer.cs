using System.Threading.Tasks;
using Planes262.Networking;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class GameInitializer : MonoBehaviour
    {
        private UIManager uiManager;
        private Messenger messenger;
        private Game game;

        private async void Awake()
        {
            InitializeGame();
            await InitializeNetworking();
        }

        private void InitializeGame()
        {
            uiManager = FindObjectOfType<UIManager>();
            messenger = FindObjectOfType<Messenger>();
            game = FindObjectOfType<Game>();
        }

        private async Task InitializeNetworking()
        {
            LocalJudge judge = new LocalJudge(uiManager, game);
            ServerJudge serverJudge = new ServerJudge(messenger, uiManager, game);
            
            ClientHandle clientHandle = new ClientHandle(serverJudge);
            CsWebSocketClient wsClient = new CsWebSocketClient(clientHandle);
            Client client = new Client(wsClient);
            
            game.MoveAttempted += (sender, args) => client.MoveTroop(args.Position, args.Direction);
            uiManager.GameJoined += (sender, username) => client.JoinGame(username);
            messenger.MessageSent += (sender, message) => client.SendMessage(message);
            
            await wsClient.InitializeConnection();
            await wsClient.BeginListenAsync();
        }
    }
}