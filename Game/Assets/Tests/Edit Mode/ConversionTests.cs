using GameDataStructures.Positioning;
using NUnit.Framework;
using Planes262.LevelEditor.Tilemaps;
using UnityEngine;

namespace LevelEditor.Tests.EditMode
{
    public class ConversionTests
    {
        [Test]
        public void TestCubeToWorld()
        {
            Cube cube = new Cube(5, 10);

            Vector3 wp = cube.ToWorld(10f);
            Cube converted = Cube.ToCube(wp, 10f);
            
            Assert.AreEqual(cube.x, converted.x);
            Assert.AreEqual(cube.y, converted.y);
            Assert.AreEqual(cube.z, converted.z);
        }
        
        [Test]
        public void TestCubeToOffset()
        {
            Cube cube = new Cube(5, 10);

            VectorTwo offset = cube.ToOffset();
            Cube converted = Cube.FromOffset(offset);
            
            Assert.AreEqual(cube.x, converted.x);
            Assert.AreEqual(cube.y, converted.y);
            Assert.AreEqual(cube.z, converted.z);
        }
    }
}
