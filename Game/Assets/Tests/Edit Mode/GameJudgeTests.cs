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

            gc.TroopMoved += Debug.Log;
            gc.TroopsSpawned += Debug.Log;
            gc.GameEnded += Debug.Log;
            
            gc.BeginGame();
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 5), 0);
        }
    }
}
