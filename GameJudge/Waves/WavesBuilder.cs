using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge.Waves
{
    public class WavesBuilder
    {
        private readonly Dictionary<int, List<TroopDto>> troopsForRound = new Dictionary<int, List<TroopDto>>();

        private int maxRedWave;
        private int maxBlueWave;

        public WavesBuilder Add(int round, int x, int y, PlayerSide player)
        {
            SetMaxWave(player, round);
            AddTroopToRound(round, new VectorTwo(x, y), player);
            return this;
        }

        private void SetMaxWave(PlayerSide player, int round)
        {
            if (player == PlayerSide.Red)
                if (round > maxRedWave)
                    maxRedWave = round;
            if (player == PlayerSide.Blue)
                if (round > maxBlueWave)
                    maxBlueWave = round;
        }

        private void AddTroopToRound(int round, VectorTwo p, PlayerSide player)
        {
            TroopDto troop = player == PlayerSide.Red ? TroopFactory.RedDto(p.X, p.Y) : TroopFactory.BlueDto(p.X, p.Y);

            try
            {
                troopsForRound[round].Add(troop);
            }
            catch (KeyNotFoundException)
            {
                troopsForRound[round] = new List<TroopDto> {troop};
            }
        }

        public WaveProvider GetWaves()
        {
            return new WaveProvider(troopsForRound, maxRedWave, maxBlueWave);
        }
    }
}
