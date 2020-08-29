namespace Planes262.GameLogic
{
    public class Score
    {
        private int red;
        private int blue;

        public virtual void Increment(PlayerSide player)
        {
            if (player == PlayerSide.Red) red++;
            if (player == PlayerSide.Blue) blue++;
        }

        public virtual void Reset()
        {
            red = 0;
            blue = 0;
        }

        public override string ToString()
        {
            return $"{red} : {blue}";
        }
    }
}
