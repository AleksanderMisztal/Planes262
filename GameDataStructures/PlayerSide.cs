namespace GameDataStructures
{
    public enum PlayerSide
    {
        Blue = 0,
        Red = 1
    }

    public static class PlayerIdExtensions
    {
        public static PlayerSide Opponent(this PlayerSide player)
        {
            return player == PlayerSide.Red ? PlayerSide.Blue : PlayerSide.Red;
        }
    }
}