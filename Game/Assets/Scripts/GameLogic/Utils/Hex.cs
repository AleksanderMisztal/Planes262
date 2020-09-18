using GameDataStructures;

namespace Planes262.GameLogic.Utils
{
    public static class Hex
    {
        public static VectorTwo GetAdjacentHex(VectorTwo cell, int direction)
        {
            return new HexOffset(cell).GetAdjacentHex(direction).ToVector();
        }

        public static VectorTwo[] GetControlZone(VectorTwo cell, int orientation)
        {
            VectorTwo[] cells = new VectorTwo[3];
            for (int i = -1; i < 2; i++)
            {
                int direction = (orientation + i + 6) % 6;
                cells[i + 1] = GetAdjacentHex(cell, direction);
            }
            return cells;
        }
    }
}
