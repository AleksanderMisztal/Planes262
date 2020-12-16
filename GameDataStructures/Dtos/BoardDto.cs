using System;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class BoardDto
    {
        public int xSize;
        public int ySize;

        public BoardDto(int xSize, int ySize)
        {
            this.xSize = xSize;
            this.ySize = ySize;
        }

        public BoardDto() { }

        public Board Get() => new Board(xSize, ySize);
    }
}