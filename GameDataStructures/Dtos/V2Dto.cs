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
    
    public static class V2Ext
    {
        public static V2Dto Dto(this VectorTwo v)
        {
            return new V2Dto {x = v.x, y = v.y};
        }
    }
}