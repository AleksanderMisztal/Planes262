using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Waves;
using NUnit.Framework;
using UnityEngine;

namespace Planes262.Tests.Edit_Mode
{
    public class GameJudgeTests
    {
        [Test]
        public void TestGameJudge()
        {
            GameController gc = new GameController(WaveProvider.Basic(), Board.Standard);

            gc.TroopMoved += Debug.Log;
            gc.TroopsSpawned += Debug.Log;
            gc.GameEnded += Debug.Log;
            
            gc.BeginGame();
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 5), 0);
        }
    }
}
