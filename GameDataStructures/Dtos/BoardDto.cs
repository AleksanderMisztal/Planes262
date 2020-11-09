using System;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class BoardDto
    {
        public int xSize;
        public int ySize;
        
        public Board Get() => new Board(xSize, ySize);
    }
}