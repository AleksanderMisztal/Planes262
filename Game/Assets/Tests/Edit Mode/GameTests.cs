using NUnit.Framework;
using Planes262.GameLogic;
using Planes262.UnityLayer;

namespace Planes262.Tests.Edit_Mode
{
    public class GameTests
    {
        [Test]
        public void GameTestsSimplePasses()
        {
            TroopManager troopManager = new TroopManager();
            MapController mapController = null;
            Game game = new Game(troopManager, mapController);
            
        }
    }
}
