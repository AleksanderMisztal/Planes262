namespace Planes262.GameLogic.Utils
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

        public static GameJudge.PlayerSide ToJudge(this PlayerSide side)
        {
            return side == PlayerSide.Red ? GameJudge.PlayerSide.Red : GameJudge.PlayerSide.Blue;
        }
    }
}