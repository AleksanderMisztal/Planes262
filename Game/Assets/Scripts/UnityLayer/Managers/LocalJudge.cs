using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameJudge;
using GameJudge.WavesN;
using Planes262.GameLogic.Troops;

namespace Planes262.UnityLayer.Managers
{
    public class LocalJudge
    {
        public LocalJudge(UIManager uiManager, GameManager gameManager)
        {
            uiManager.LocalPlayed += () => Initialize(uiManager, gameManager);
        }

        private static void Initialize(UIManager uiManager, GameManager gameManager)
        {
            GameController gameController = new GameController(Waves.Test(), Board.Test);
            
            gameController.TroopMoved += args => gameManager.MoveTroop(args.Position, args.Direction, args.BattleResults);
            gameController.TroopsSpawned += args => gameManager.BeginNextRound(args.Troops.ToUTroop());
            gameController.GameEnded += args => uiManager.EndGame(args.Score.ToString(), 1.5f);

            gameManager.MoveAttempted += args => gameController.ProcessMove(args.Side, args.Position, args.Direction);
            
            gameManager.SetLocal(true);
            gameManager.StartNewGame(Board.Test, PlayerSide.Blue);
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