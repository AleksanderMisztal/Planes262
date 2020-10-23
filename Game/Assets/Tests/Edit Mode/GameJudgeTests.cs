using System.Diagnostics;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Troops;
using GameJudge.Waves;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

namespace Planes262.Tests.Edit_Mode
{
    public class GameJudgeTests
    {
        [Test]
        public void TestGameJudge()
        {
            GameController gc = new GameController(WaveProvider.Basic(), Board.standard);

            gc.TroopMoved += Debug.Log;
            gc.TroopsSpawned += Debug.Log;
            gc.GameEnded += Debug.Log;
            
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 5), 0);
        }
        
        [Test]
        public void TestReturningCopies()
        {
            GameController gc = new GameController(WaveProvider.Test(), Board.test);
            Troop troop = null;
            bool wasLegal = false;
            gc.TroopsSpawned += args => troop = args.troops.ToList()[0];
            gc.TroopsSpawned += args => Trace.WriteLine(args);
            gc.TroopMoved += args => wasLegal = true;
            
            VectorTwo position = troop.Position;
            troop.MoveInDirection(0);
            gc.ProcessMove(PlayerSide.Blue, position, 0);
            
            Assert.IsTrue(wasLegal);
        }
    }
}
