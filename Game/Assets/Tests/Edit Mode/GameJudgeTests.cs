using GameDataStructures;
using GameJudge;
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
            GameController gc = new GameController(Waves.Basic(), Board.Standard);

            gc.TroopMoved += (sender, args) => Debug.Log(args);
            gc.TroopsSpawned += (sender, args) => Debug.Log(args);
            gc.GameEnded += (sender, args) => Debug.Log(args);
            
            gc.BeginGame();
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 5), 0);
        }
    }
}
