using GameDataStructures;
using GameJudge.Waves;
using UnityEngine;

namespace Planes262.Saving
{
    public class LevelData
    {
        public readonly Vector3 offset;
        public readonly float gridSize;
        public readonly string background;
        public readonly WaveProvider waveProvider;
        public readonly Board board;
        public readonly string[] terrain;

        public LevelData(Vector3 offset, float gridSize, string background, WaveProvider waveProvider, Board board, string[] terrain)
        {
            this.offset = offset;
            this.gridSize = gridSize;
            this.background = background;
            this.waveProvider = waveProvider;
            this.board = board;
            this.terrain = terrain;
        }
    }
}