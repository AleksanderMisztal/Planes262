using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Planes262.GameLogic.Troops;
using Planes262.Networking;
using UnityEngine;
using PlayerSide = GameJudge.PlayerSide;
using VectorTwo = GameJudge.Utils.VectorTwo;

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

    public static class TroopListExtensions
    {
        public static IEnumerable<Troop> ToUTroops(this List<GameJudge.Troops.Troop> troops)
        {
            return troops.Select(t => new Troop(t.Player.ToUps(), t.MovePoints, t.Position.ToUVec(), t.Orientation, t.Health));
        }

        public static GameLogic.Utils.VectorTwo ToUVec(this VectorTwo v)
        {
            return new GameLogic.Utils.VectorTwo(v.X, v.Y);
        }

        public static GameLogic.Utils.PlayerSide ToUps(this PlayerSide side)
        {
            return side == PlayerSide.Red ? GameLogic.Utils.PlayerSide.Red : GameLogic.Utils.PlayerSide.Blue;
        }
    }
}