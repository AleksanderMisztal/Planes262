using NUnit.Framework;
using Planes262.GameLogic;
using Planes262.GameLogic.Area;
using Planes262.GameLogic.Data;
using Planes262.GameLogic.Troops;

namespace Planes262.Tests.Edit_Mode
{
    public class ScoringTroopTests
    {
        [Test]
        public void TestDamageScoringTroop()
        {
            Score score = new Score();
            Troop troop = TroopFactory.Blue(3, 4);
            TroopDecorator st = new DamageScoringTroopDecorator(score, troop);
            
            st.ApplyDamage();
            st.ApplyDamage();
            
            Assert.AreEqual(score.Red, 2);
            Assert.AreEqual(score.Blue, 0);
        }
        
        [Test]
        public void TestAreaScoringTroop()
        {
            Score score = new Score();
            IArea area = new VerticalLineArea(5, false);
            Troop troop = TroopFactory.Blue(4, 4);
            TroopDecorator st = new AreaScoringTroopDecorator(score, area, 2, troop);
            
            st.MoveInDirection(0);
            Assert.AreEqual(score.Blue, 0);
            
            st.MoveInDirection(0);
            Assert.AreEqual(score.Blue, 2);
            
            st.MoveInDirection(0);
            Assert.AreEqual(score.Blue, 2);
        }
    }
}