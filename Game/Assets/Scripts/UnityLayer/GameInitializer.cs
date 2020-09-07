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
        
        private ServerInputManager serverInputManager;

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
            
            serverInputManager = new ServerInputManager(messenger, uiManager, game);
        }

        private async Task InitializeNetworking()
        {
            // TODO: judge = new ServerJudge(game); move rest into ServerJudge
            ClientHandle clientHandle = new ClientHandle(serverInputManager);
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