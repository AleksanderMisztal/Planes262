using GameDataStructures;
using NUnit.Framework;
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
    }
}