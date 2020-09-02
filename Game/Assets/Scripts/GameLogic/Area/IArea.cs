using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic.Area
{
    public interface IArea
    {
        bool IsInside(VectorTwo position);
    }
}