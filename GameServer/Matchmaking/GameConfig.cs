using System.Collections.Generic;
using GameDataStructures.Dtos;
using GameServer.Utils;

namespace GameServer.Matchmaking
{
    public class GameConfig
    {
        public readonly LevelDto levelDto;
        public readonly int time;
        public readonly int increment;
        
        private GameConfig(LevelDto levelDto, int time, int increment)
        {
            this.levelDto = levelDto;
            this.time = time;
            this.increment = increment;
        }
       
        
        public static readonly Dictionary<string, GameConfig> configs = new Dictionary<string, GameConfig>();

        static GameConfig()
        {
            configs.Add("test", Test());
            configs.Add("basic", Basic());
        }

        private const string level0 ="{\"background\":\"board\",\"board\":{\"xSize\":15,\"ySize\":10},\"cameraDto\":{\"xOffset\":13.890222549438477,\"yOffset\":8.060089111328125,\"ortoSize\":12.500011444091797},\"troopDtos\":[{\"name\":\"Liberator\",\"type\":0,\"side\":1,\"position\":{\"x\":7,\"y\":5},\"orientation\":3,\"movePoints\":5,\"health\":2},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":2,\"y\":6},\"orientation\":0,\"movePoints\":5,\"health\":2},{\"name\":\"Liberator\",\"type\":0,\"side\":1,\"position\":{\"x\":7,\"y\":6},\"orientation\":3,\"movePoints\":5,\"health\":2},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":2,\"y\":7},\"orientation\":0,\"movePoints\":5,\"health\":2}]}";
        
        private static GameConfig Test() =>
            new(Json.Read(level0), 30, 5);
        
        private static GameConfig Basic() =>
            new(Json.Read(level0), 60, 20);
    }
}