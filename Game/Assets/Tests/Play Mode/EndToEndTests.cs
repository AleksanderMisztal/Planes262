using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using Planes262.UnityLayer;
using UnityEngine.TestTools;

namespace Planes262.Tests.Play_Mode
{
    public class EndToEndTests
    {
        private void Click(int x, int y)
        {
            VectorTwo cell = new VectorTwo(x, y);
            //MapController.OnCellClicked(cell);
        }
        
        [UnityTest]
        public IEnumerator EndToEndTestsWithEnumeratorPasses()
        {
            
            yield return null;
        }
    }
}
