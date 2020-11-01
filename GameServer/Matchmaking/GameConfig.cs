using System.Collections.Generic;
using GameDataStructures;
using GameJudge.Troops;
using GameJudge.Waves;

namespace GameServer.Matchmaking
{
    public class GameConfig
    {
        public readonly WaveProvider waveProvider;
        public readonly Board board;
        public readonly int time;
        public readonly int increment;
        
        private GameConfig(WaveProvider waveProvider, Board board, int time, int increment)
        {
            this.waveProvider = waveProvider;
            this.board = board;
            this.time = time;
            this.increment = increment;
        }
       
        
        public static readonly Dictionary<string, GameConfig> configs = new Dictionary<string, GameConfig>();

        static GameConfig()
        {
            configs.Add("test", Test());
            configs.Add("basic", Basic());
        }


        private static GameConfig Test() =>
            new GameConfig(
                new WaveProvider(new List<Troop> {
                    TroopFactory.Blue(1, 3),
                    TroopFactory.Red(5, 3),
                }),
                new Board(12, 7),
                30, 5);
        
        private static GameConfig Basic() =>
            new GameConfig(
                new WaveProvider(new List<Troop> {
                    TroopFactory.Blue(1, 3),
                    TroopFactory.Blue(1, 4),
                    TroopFactory.Blue(1, 5),
                    TroopFactory.Blue(1, 6),
                    TroopFactory.Red(5, 4),
                    TroopFactory.Red(5, 5),
                    TroopFactory.Red(5, 6),
                    TroopFactory.Red(5, 7),
                }),
                new Board(16, 10),
                60, 20);
    }
}