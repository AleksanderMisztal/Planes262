using GameJudge;
using GameJudge.Areas;
using GameJudge.Battles;
using GameJudge.Utils;
using GameJudge.WavesN;
using NUnit.Framework;
using UnityEngine;

namespace Planes262.Tests.Edit_Mode
{
    public class GameJudgeTests
    {
        [Test]
        public void GameJudgeTestsSimplePasses()
        {
            GameController gc = new GameController(new AlwaysDamageBattles(), Board.Standard, Waves.Basic());

            gc.TroopMoved += (sender, args) => Debug.Log(args);
            gc.TroopsSpawned += (sender, args) => Debug.Log(args);
            gc.GameEnded += (sender, args) => Debug.Log(args);
            
            gc.BeginGame();
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 5), 0);
        }
    }
}
