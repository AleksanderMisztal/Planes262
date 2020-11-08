using System.Collections.Generic;
using GameDataStructures;

namespace GameJudge.Waves
{
    public class WaveProvider
    {
        public readonly TroopDto[] initialTroops;
        private readonly Dictionary<int, List<TroopDto>> troopsForRound;

        public WaveProvider(TroopDto[] initialTroops, Dictionary<int, List<TroopDto>> troopsForRound)
        {
            this.initialTroops = initialTroops;
            this.troopsForRound = troopsForRound;
        }

        public WaveProvider(TroopDto[] initialTroops)
        {
            this.initialTroops = initialTroops;
            troopsForRound = new Dictionary<int, List<TroopDto>>();
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
    }
}