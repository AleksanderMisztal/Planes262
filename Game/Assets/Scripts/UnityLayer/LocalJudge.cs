using GameJudge;
using GameJudge.Areas;
using GameJudge.Utils;
using GameJudge.WavesN;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class LocalJudge
    {
        public LocalJudge(UIManager uiManager, Game game)
        {
            uiManager.LocalPlayed += (sender, args) => Initialize(uiManager, game);
        }

        private static void Initialize(UIManager uiManager, Game game)
        {
            GameController gameController = new GameController(Waves.Test(), Board.Standard);
            
            gameController.TroopMoved += (sender, args) => game.MoveTroop(args.Position.ToUVec(), args.Direction, args.BattleResults);
            gameController.TroopMoved += (sender, args) => Debug.Log(args);
            gameController.TroopsSpawned += (sender, args) => game.BeginNextRound(args.Troops.ToUTroops());
            gameController.GameEnded += (sender, args) => uiManager.EndGame(args.Score.ToString(), 1.5f);
            
            game.MoveAttempted += (sender, args) =>
                gameController.ProcessMove(GameLogic.Utils.UPlayerSideExtensions.ToJudge(args.Side),
                    new VectorTwo(args.Position.X, args.Position.Y), args.Direction);
            
            game.SetLocal(true);
            game.StartNewGame(GameLogic.Area.Board.Standard, GameLogic.Utils.PlayerSide.Blue);
            gameController.BeginGame();
        }
    }
}