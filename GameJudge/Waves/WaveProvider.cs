using System.Collections.Generic;
using System.Linq;
using GameJudge.Troops;

namespace GameJudge.Waves
{
    public class WaveProvider
    {
        public readonly List<Troop> initialTroops;
        private readonly Dictionary<int, List<Troop>> troopsForRound;

        public WaveProvider(List<Troop> initialTroops, Dictionary<int, List<Troop>> troopsForRound)
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
    }

    public static class WaveExtensions
    {
        public static IEnumerable<Troop> Copy(this IEnumerable<Troop> troops)
        {
            return troops.Select(t => t.Copy());
        }
    }
}