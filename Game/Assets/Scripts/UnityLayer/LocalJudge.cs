using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameJudge;
using GameJudge.WavesN;
using Planes262.GameLogic.Troops;

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
            GameController gameController = new GameController(Waves.Test(), Board.Test);
            
            gameController.TroopMoved += (sender, args) => game.MoveTroop(args.Position, args.Direction, args.BattleResults);
            gameController.TroopsSpawned += (sender, args) => game.BeginNextRound(args.Troops.ToUTroop());
            gameController.GameEnded += (sender, args) => uiManager.EndGame(args.Score.ToString(), 1.5f);
            
            game.MoveAttempted += (sender, args) =>
                gameController.ProcessMove(args.Side,
                    new VectorTwo(args.Position.X, args.Position.Y), args.Direction);
            
            game.SetLocal(true);
            game.StartNewGame(Board.Standard, PlayerSide.Blue);
            gameController.BeginGame();
        }
    }

    public static class TroopListExtension
    {
        public static  IEnumerable<Troop> ToUTroop(this IEnumerable<TroopDto> troops)
        {
            return troops.Select(t => new Troop(t.Player, t.InitialMovePoints, t.Position, t.Orientation, t.Health));
        }
    }
}