using System.Collections.Generic;
using GameDataStructures;
using GameJudge.Troops;

namespace GameJudge.Waves
{
    public class WaveProvider
    {
        private readonly Dictionary<int, List<TroopDto>> troopsForRound;

        public readonly int MaxRedWave;
        public readonly int MaxBlueWave;

        internal WaveProvider(Dictionary<int, List<TroopDto>> troopsForRound, int maxRedWave, int maxBlueWave)
        {
            this.troopsForRound = troopsForRound;
            MaxRedWave = maxRedWave;
            MaxBlueWave = maxBlueWave;
        }

        internal List<TroopDto> GetTroops(int round)
        {
            try
            {
                return troopsForRound[round];
            }
            catch (KeyNotFoundException)
            {
                return new List<TroopDto>();
            }
        }


        public static WaveProvider Test()
        {
            List<TroopDto> wave1 = new List<TroopDto>
            {
                TroopFactory.BlueDto(2, 3),
                TroopFactory.RedDto(6, 2),
                TroopFactory.RedDto(6, 3),
            };
            List<TroopDto> wave3 = new List<TroopDto>
            {
                TroopFactory.BlueDto(2, 2),
            };

            Dictionary<int, List<TroopDto>> troopsForRound = new Dictionary<int, List<TroopDto>>
            {
                {1, wave1 },
                {3, wave3 },
            };

            int maxRedWave = 1;
            int maxBlueWave = 3;

            return new WaveProvider(troopsForRound, maxRedWave, maxBlueWave);
        }
        
        public static WaveProvider Basic()
        {
            List<TroopDto> wave1 = new List<TroopDto>
            {
                TroopFactory.BlueDto(2, 5),
                TroopFactory.BlueDto(2, 6),
                TroopFactory.BlueDto(2, 7),
                TroopFactory.BlueDto(2, 8),
                TroopFactory.RedDto(16, 4),
                TroopFactory.RedDto(16, 5),
                TroopFactory.RedDto(16, 6),
                TroopFactory.RedDto(16, 7),
                TroopFactory.RedDto(16, 8),
            };
            List<TroopDto> wave3 = new List<TroopDto>
            {
                TroopFactory.BlueDto(2, 5),
                TroopFactory.BlueDto(2, 6),
                TroopFactory.BlueDto(2, 7),
                TroopFactory.BlueDto(2, 8),
            };
            List<TroopDto> wave4 = new List<TroopDto>
            {
                TroopFactory.RedDto(16, 4),
                TroopFactory.RedDto(16, 5),
                TroopFactory.RedDto(16, 6),
                TroopFactory.RedDto(16, 7),
            };
            List<TroopDto> wave5 = new List<TroopDto>
            {
                TroopFactory.BlueDto(2, 5),
                TroopFactory.BlueDto(2, 6),
                TroopFactory.BlueDto(2, 7),
                TroopFactory.BlueDto(2, 8),
            };
            List<TroopDto> wave6 = new List<TroopDto>
            {
                TroopFactory.RedDto(16, 5),
                TroopFactory.RedDto(16, 6),
                TroopFactory.RedDto(16, 7),
            };

            Dictionary<int, List<TroopDto>> troopsForRound = new Dictionary<int, List<TroopDto>>
            {
                {1, wave1 },
                {3, wave3 },
                {4, wave4 },
                {5, wave5 },
                {6, wave6 },
            };

            int maxRedWave = 6;
            int maxBlueWave = 5;

            return new WaveProvider(troopsForRound, maxRedWave, maxBlueWave);
        }
    }
}