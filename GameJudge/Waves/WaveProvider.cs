using System.Collections.Generic;
using System.Linq;
using GameJudge.Troops;

namespace GameJudge.Waves
{
    public class WaveProvider
    {
        public readonly List<Troop> initialTroops;
        private readonly Dictionary<int, List<Troop>> troopsForRound;

        internal WaveProvider(List<Troop> initialTroops, Dictionary<int, List<Troop>> troopsForRound)
        {
            this.initialTroops = initialTroops;
            this.troopsForRound = troopsForRound;
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
                {3, wave3 },
            };

            return new WaveProvider(wave1, troopsForRound);
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
                {3, wave3 },
                {4, wave4 },
                {5, wave5 },
                {6, wave6 },
            };

            return new WaveProvider(wave1, troopsForRound);
        }
    }

    public static class WaveExtensions
    {
        public static IEnumerable<Troop> Copy(this IEnumerable<Troop> troops)
        {
            return troops.Select(t => t.Copy());
        }
    }
}