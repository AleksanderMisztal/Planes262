namespace GameDataStructures
{
    public class Score
    {
        public int Red { get; private set; }
        public int Blue { get; private set; }

        public virtual void Increment(PlayerSide player, int amount)
        {
            if (player == PlayerSide.Red) Red += amount;
            if (player == PlayerSide.Blue) Blue += amount;
        }

        public virtual void Increment(PlayerSide player)
        {
            Increment(player, 1);
        }

        public virtual void Reset()
        {
            Red = 0;
            Blue = 0;
        }

        public override string ToString()
        {
            return $"{Red} : {Blue}";
        }
    }
}