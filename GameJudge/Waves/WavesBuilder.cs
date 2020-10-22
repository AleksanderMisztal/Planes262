using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Waves
{
    public class WavesBuilder
    {
        private readonly Dictionary<int, List<Troop>> troopsForRound = new Dictionary<int, List<Troop>>();

        public WavesBuilder Add(int round, int x, int y, PlayerSide player)
        {
            AddTroopToRound(round, new VectorTwo(x, y), player);
            return this;
        }

        private void AddTroopToRound(int round, VectorTwo p, PlayerSide player)
        {
            Troop troop = player == PlayerSide.Red ? TroopFactory.Red(p.x, p.y) : TroopFactory.Blue(p.x, p.y);

            try
            {
                troopsForRound[round].Add(troop);
            }
            catch (KeyNotFoundException)
            {
                troopsForRound[round] = new List<Troop> {troop};
            }
        }

        public WaveProvider GetWaves()
        {
            List<Troop> wave1 = troopsForRound[1];
            troopsForRound.Remove(1);
            return new WaveProvider(wave1, troopsForRound);
        }
    }
}
