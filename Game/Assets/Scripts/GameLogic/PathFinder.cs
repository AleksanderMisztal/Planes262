using System.Linq;
using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;

namespace Assets.Scripts.GameLogic
{
    public class PathFinder
    {
        private readonly TroopMap map;

        private readonly Dictionary<Coord, Coord> parent = new Dictionary<Coord, Coord>();
        private readonly Dictionary<VectorTwo, Coord> orient = new Dictionary<VectorTwo, Coord>();

        public PathFinder(TroopMap map)
        {
            this.map = map;
        }


        public HashSet<VectorTwo> GetReachableCells(VectorTwo position)
        {
            Troop troop = map.Get(position);
            if (troop == null) throw new PathFindingException("No troop on this hex!");
            return GetReachableCells(troop);
        }

        private HashSet<VectorTwo> GetReachableCells(Troop troop)
        {
            HashSet<Coord> coords = new HashSet<Coord>();
            Coord initialPosition = new Coord(troop.Position, troop.Orientation);
            coords.Add(initialPosition);
            for (int i = 0; i < troop.MovePoints; i++)
                coords = GetNextLayer(troop.Player, coords);
            coords.Remove(initialPosition);
            return new HashSet<VectorTwo>(coords.Select(c => c.Position));
        }

        private HashSet<Coord> GetNextLayer(PlayerSide side, HashSet<Coord> coords)
        {
            HashSet<Coord> acm = new HashSet<Coord>();
            foreach (var coord in coords)
            {
                acm.Add(coord);
                if (map.Get(coord.Position) != null) continue;
                foreach (var cell in coord.GetControllZone())
                {
                    Troop encounter = map.Get(cell.Position);
                    if (encounter == null || encounter.Player != side)
                    {
                        acm.Add(cell);
                        orient[cell.Position] = cell;
                    }
                }
            }
            return acm;
        }


        public List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            List<int> directions = new List<int>();
            Coord coords = orient[end];
            while (coords.Position != start)
            {
                Coord prevCoords = parent[coords];
                int direction = prevCoords.GetDirection(coords);
                directions.Add(direction);
                coords = prevCoords;
            }
            return directions;
        }
    }
}
