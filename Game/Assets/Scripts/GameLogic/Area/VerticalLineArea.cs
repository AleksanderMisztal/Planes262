using Planes262.GameLogic.Troops;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic.Area
{
    public class VerticalLineArea : IArea
    {
        private readonly int x;
        private readonly bool toLeft;

        public VerticalLineArea(int x, bool toLeft)
        {
            this.x = x;
            this.toLeft = toLeft;
        }


        public bool IsInside(VectorTwo position)
        {
            if (toLeft) return position.X < x;
            return position.X > x;
        }
    }
}