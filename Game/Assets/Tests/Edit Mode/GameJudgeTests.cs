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
            gc.ProcessMove(PlayerSide.Blue, new VectorTwo(2, 5), 0);
        }
        
        [Test]
        public void TestReturningCopies()
        {
            WaveProvider waveProvider = WaveProvider.Test();
            GameController gc = new GameController(waveProvider, Board.test);
            Troop troop = waveProvider.initialTroops[0];
            VectorTwo position = troop.Position;
            
            gc.ProcessMove(PlayerSide.Blue, position, 0);
            
            Assert.AreEqual(position, troop.Position);
        }
    }
}
