using NUnit.Framework;
using Planes262.GameLogic;

namespace Planes262.Tests.Edit_Mode
{
    public class TroopManagerTests
    {
        [Test]
        public void GameTestsSimplePasses()
        {
            TroopMap troopMap = new TroopMap();
            TroopManager troopManager = new TroopManager(troopMap);
            
        }
    }
}
