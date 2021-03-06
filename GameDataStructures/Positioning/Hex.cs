﻿using System.Collections.Generic;
using System.Linq;

namespace GameDataStructures.Positioning
{
    public static class Hex
    {
        public static VectorTwo GetAdjacentHex(VectorTwo cell, int direction)
        {
            return new HexOffset(cell).GetAdjacentHex(direction).ToVector();
        }

        public static IEnumerable<VectorTwo> GetNeighbours(VectorTwo cell)
        {
            return new HexOffset(cell).GetNeighbors().Select(c => c.ToVector());
        }

        public static IEnumerable<VectorTwo> GetControlZone(VectorTwo cell, int orientation)
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
