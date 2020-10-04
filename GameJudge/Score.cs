using GameDataStructures;

namespace GameJudge
{
    public class Score
    {
        public int Red { get; private set; }
        public int Blue { get; private set; }

        public void Increment(PlayerSide player, int amount = 1)
        {
            if (player == PlayerSide.Red) Red += amount;
            if (player == PlayerSide.Blue) Blue += amount;
        }

        public override string ToString()
        {
            return $"{Red} : {Blue}";
        }
    }
}