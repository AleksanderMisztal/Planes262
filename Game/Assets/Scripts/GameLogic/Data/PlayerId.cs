namespace Planes262.GameLogic.Data
{
    public enum PlayerSide
    {
        Blue = 0,
        Red = 1
    }

    public static class PlayerSideExtensions
    {
        public static PlayerSide Opponent(this PlayerSide player)
        {
            return player == PlayerSide.Red ? PlayerSide.Blue : PlayerSide.Red;
        }
    }
}