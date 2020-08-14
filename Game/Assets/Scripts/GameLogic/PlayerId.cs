namespace GameServer.GameLogic
{
    public enum PlayerSide
    {
        Blue = 0,
        Red = 1
    }

    static class PlayerSideExtensions
    {
        public static PlayerSide Opponent(this PlayerSide player)
        {
            return player == PlayerSide.Red ? PlayerSide.Blue : PlayerSide.Red;
        }
    }
}