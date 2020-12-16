using System;
using GameDataStructures.Positioning;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class V2Dto
    {
        public V2Dto(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;

        public V2Dto() { }

        public VectorTwo Get() => new VectorTwo(x, y);
    }
    
    public static class V2Ext
    {
        public static V2Dto Dto(this VectorTwo v)
        {
            return new V2Dto(v.x,v.y);
        }
    }
}