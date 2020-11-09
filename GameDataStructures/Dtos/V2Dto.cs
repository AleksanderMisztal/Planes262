using System;
using GameDataStructures.Positioning;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class V2Dto
    {
        public int x;
        public int y;
        
        public VectorTwo Get() => new VectorTwo(x, y);
    }
}