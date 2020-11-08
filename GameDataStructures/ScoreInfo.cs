using System;

namespace GameDataStructures
{
    [Serializable]
    public class ScoreInfo
    {
        public int Red { get; }
        public int Blue { get; }

        public ScoreInfo(int red, int blue)
        {
            Red = red;
            Blue = blue;
        }
    }
}