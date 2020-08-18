namespace Planes262.GameLogic
{
    public class TroopDto
    {
        public readonly PlayerSide side;
        public readonly int orientation;

        public TroopDto(PlayerSide side, int orientation)
        {
            this.side = side;
            this.orientation = orientation;
        }
    }
}
