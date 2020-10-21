using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;

namespace Planes262.UnityLayer.Managers
{
    public interface ITileManager
    {
        void CreateBoard(Board board);
        void SetReachableTiles(IEnumerable<VectorTwo> reachable);
        void SetReachableTilesBlocked(HashSet<VectorTwo> reachable);
        void HighlightPath(IEnumerable<VectorTwo> path);
        void ResetAllTiles();
    }
}