using System.Linq;
using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;

namespace Assets.Scripts.GameLogic
{
    public class PathFinder
    {
        private readonly TroopMap map;

        private Dictionary<Coords, Coords> parent = new Dictionary<Coords, Coords>();
        private Dictionary<VectorTwo, Coords> orient = new Dictionary<VectorTwo, Coords>();

        public PathFinder(TroopMap map)
        {
            this.map = map;
        }

        public IEnumerable<VectorTwo> GetReachableCells(VectorTwo position)
        {
            Troop troop = map.Get(position);
            if (troop == null) throw new PathFindingException("No troop on this hex!");

            HashSet<Coords> coords = new HashSet<Coords>
            {
                new Coords(troop.Position, troop.Orientation)
            };
            for (int i = 0; i <= troop.MovePoints; i++)
            {
                var acm = new HashSet<Coords>();
                foreach (var c in coords)
                {
                    acm.Add(c);
                    if (map.Get(c.Position) != null) continue;
                    foreach (var cell in c.GetControllZone())
                    {
                        Troop encounter = map.Get(cell.Position);
                        if (encounter == null || encounter.Player != troop.Player)
                            acm.Add(cell);
                    }
                }
                coords = acm;
            }
            return coords.Select(c => c.Position);
        }

        public List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            List<int> directions = new List<int>();
            Coords coords = orient[end];
            while (coords.Position != start)
            {
                Coords prevCoords = parent[coords];
                int direction = prevCoords.GetDirection(coords);
                directions.Add(direction);
                coords = prevCoords;
            }
            return directions;
        }
    }
}
