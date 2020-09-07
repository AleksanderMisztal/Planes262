using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameJudge;
using GameJudge.Areas;
using GameJudge.WavesN;
using Planes262.GameLogic.Troops;
using Planes262.GameLogic.Utils;
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
        
        private ServerJudge serverJudge;

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
            
            serverJudge = new ServerJudge(messenger, uiManager, game);
            
            
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

    public class LocalJudge
    {
        private readonly UIManager uiManager;
        private readonly Game game;
        private readonly GameController gameController;

        public LocalJudge(UIManager uiManager, Game game)
        {
            this.uiManager = uiManager;
            this.game = game;
            
            gameController = new GameController(Waves.Basic(), Board.Standard);

            game.MoveAttempted += (sender, args) =>
                gameController.ProcessMove(args.Side.ToJudge(), new VectorTwo(args.Position.X, args.Position.Y), args.Direction);
            uiManager.LocalPlayed += (sender, args) => gameController.BeginGame();
            
            gameController.TroopMoved += (sender, args) =>
                game.MoveTroop(args.Position.ToUVec(), args.Direction, args.BattleResults);
            gameController.TroopsSpawned += (sender, args) => game.BeginNextRound(args.Troops.ToUTroops());
            gameController.GameEnded += (sender, args) => uiManager.EndGame(args.Score.ToString(), 1.5f);
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