using System;

namespace GameDataStructures.Dtos
{    
    [Serializable]
    public class CameraDto
    {
        public CameraDto(float xOffset, float yOffset, float ortoSize)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.ortoSize = ortoSize;
        }

        public CameraDto() { }

        public float xOffset;
        public float yOffset;
        public float ortoSize;
    }
}