using System;
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

        public void Print()
        {
            Console.WriteLine("Printing level config");
            Console.WriteLine(levelDto);
            Console.WriteLine(levelDto.background);
            Console.WriteLine(levelDto.board);
            Console.WriteLine(levelDto.troopDtos);
        }

        private const string level0 = "{\"background\":\"board\",\"board\":{\"xSize\":18,\"ySize\":10},\"cameraDto\":{\"xOffset\":13.190206527709961,\"yOffset\":8.800106048583985,\"ortoSize\":12.0},\"troopDtos\":[{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":10,\"y\":3},\"orientation\":3,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":3},\"orientation\":3,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":4},\"orientation\":1,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":12,\"y\":4},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":4},\"orientation\":3,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":14,\"y\":4},\"orientation\":4,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":5},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":8,\"y\":5},\"orientation\":1,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":9,\"y\":5},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":10,\"y\":5},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":5},\"orientation\":4,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":6},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":8,\"y\":6},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":10,\"y\":6},\"orientation\":0,\"movePoints\":0,\"health\":0},{\"name\":\"B17\",\"type\":0,\"side\":0,\"position\":{\"x\":13,\"y\":6},\"orientation\":4,\"movePoints\":0,\"health\":0},{\"name\":\"Me262\",\"type\":0,\"side\":0,\"position\":{\"x\":6,\"y\":7},\"orientation\":0,\"movePoints\":0,\"health\":0}]}";
        
        private static GameConfig Test() =>
            new GameConfig(Json.Read(level0), 30, 5);
        
        private static GameConfig Basic() =>
            new GameConfig(Json.Read(level0), 60, 20);
    }
}