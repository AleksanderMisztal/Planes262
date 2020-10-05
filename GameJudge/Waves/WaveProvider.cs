﻿using System.Collections.Generic;
using GameJudge.Troops;

namespace GameJudge.Waves
{
    public class WaveProvider
    {
        private readonly Dictionary<int, List<Troop>> troopsForRound;

        public readonly int MaxRedWave;
        public readonly int MaxBlueWave;

        internal WaveProvider(Dictionary<int, List<Troop>> troopsForRound, int maxRedWave, int maxBlueWave)
        {
            this.troopsForRound = troopsForRound;
            MaxRedWave = maxRedWave;
            MaxBlueWave = maxBlueWave;
        }

        internal List<Troop> GetTroops(int round)
        {
            try
            {
                return troopsForRound[round];
            }
            catch (KeyNotFoundException)
            {
                return new List<Troop>();
            }
        }


        public static WaveProvider Test()
        {
            List<Troop> wave1 = new List<Troop>
            {
                TroopFactory.Blue(2, 3),
                TroopFactory.Red(6, 2),
                TroopFactory.Red(6, 3),
            };
            List<Troop> wave3 = new List<Troop>
            {
                TroopFactory.Blue(2, 2),
            };

            Dictionary<int, List<Troop>> troopsForRound = new Dictionary<int, List<Troop>>
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
            List<Troop> wave1 = new List<Troop>
            {
                TroopFactory.Blue(2, 5),
                TroopFactory.Blue(2, 6),
                TroopFactory.Blue(2, 7),
                TroopFactory.Blue(2, 8),
                TroopFactory.Red(16, 4),
                TroopFactory.Red(16, 5),
                TroopFactory.Red(16, 6),
                TroopFactory.Red(16, 7),
                TroopFactory.Red(16, 8),
            };
            List<Troop> wave3 = new List<Troop>
            {
                TroopFactory.Blue(2, 5),
                TroopFactory.Blue(2, 6),
                TroopFactory.Blue(2, 7),
                TroopFactory.Blue(2, 8),
            };
            List<Troop> wave4 = new List<Troop>
            {
                TroopFactory.Red(16, 4),
                TroopFactory.Red(16, 5),
                TroopFactory.Red(16, 6),
                TroopFactory.Red(16, 7),
            };
            List<Troop> wave5 = new List<Troop>
            {
                TroopFactory.Blue(2, 5),
                TroopFactory.Blue(2, 6),
                TroopFactory.Blue(2, 7),
                TroopFactory.Blue(2, 8),
            };
            List<Troop> wave6 = new List<Troop>
            {
                TroopFactory.Red(16, 5),
                TroopFactory.Red(16, 6),
                TroopFactory.Red(16, 7),
            };

            Dictionary<int, List<Troop>> troopsForRound = new Dictionary<int, List<Troop>>
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