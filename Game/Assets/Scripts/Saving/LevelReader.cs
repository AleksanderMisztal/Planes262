using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameJudge.Troops;
using GameJudge.Waves;

namespace Planes262.Saving
{
    public class LevelReader
    {
        public LevelReader()
        {
            ReadDictionaries();
        }
        
        private void ReadDictionaries()
        {
            // TODO
        }

        public LevelData GetLevel(string levelName)
        {
            GridDto troopDto = Saver.Read<GridDto>(levelName + "/troops");
            List<Troop> troops = troopDto.objects.Select(TroopDeserializer.FromString).ToList();
            WaveProvider waveProvider = new WaveProvider(troops, new Dictionary<int, List<Troop>>());

            BoardDto boardDto = Saver.Read<BoardDto>(levelName + "/board");
            Board board = new Board(boardDto.xSize, boardDto.ySize);
            
            GridDto terrainDto = Saver.Read<GridDto>(levelName + "/terrains");
            
            return new LevelData(boardDto.offset, boardDto.gridSize, boardDto.background, waveProvider, board, terrainDto.objects);
        }
    }
}