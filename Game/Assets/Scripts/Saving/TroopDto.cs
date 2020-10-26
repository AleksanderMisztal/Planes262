using System;

namespace Planes262.Saving
{
    [Serializable]
    public class TroopDto
    {
        public string name;
        public int x;
        public int y;
        public int orientation;
    }

    [Serializable]
    public class TroopDtos
    {
        public TroopDto[] dtos;
    }
}