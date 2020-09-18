using GameDataStructures;

namespace Planes262.GameLogic.Area
{
    public interface IArea
    {
        bool IsInside(VectorTwo position);
    }
}