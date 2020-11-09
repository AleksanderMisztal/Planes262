using System;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class LevelDto
    {
        public string background;
        public BoardDto board;
        public CameraDto cameraDto;
        public TroopDto[] troopDtos;
    }
}