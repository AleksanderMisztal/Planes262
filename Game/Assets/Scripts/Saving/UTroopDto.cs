using System;

namespace Planes262.Saving
{
    [Serializable]
    public class UTroopDto
    {
        public string name;
        public int x;
        public int y;
        public int orientation;
    }

    [Serializable]
    public class UTroopDtos
    {
        public UTroopDto[] dtos;
    }
}