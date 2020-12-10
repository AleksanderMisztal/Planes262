using System;

namespace GameDataStructures.Dtos
{
    [Serializable]
    public class LevelDto
    {
        public LevelDto(string background, BoardDto board, CameraDto cameraDto, TroopDto[] troopDtos)
        {
            this.background = background;
            this.board = board;
            this.cameraDto = cameraDto;
            this.troopDtos = troopDtos;
        }

        public LevelDto() { }

        public string background;
        public BoardDto board;
        public CameraDto cameraDto;
        public TroopDto[] troopDtos;
    }
}