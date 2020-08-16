using System.Linq;
using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;

namespace Assets.Scripts.GameLogic
{
    public class PathFinder
    {
        private readonly TroopMap map;

        private readonly Dictionary<OrientedCell, OrientedCell> parent = new Dictionary<OrientedCell, OrientedCell>();
        private readonly Dictionary<VectorTwo, OrientedCell> orient = new Dictionary<VectorTwo, OrientedCell>();

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
            OrientedCell initialPosition = new OrientedCell(troop.Position, troop.Orientation);
            HashSet<OrientedCell> coords = new HashSet<OrientedCell>();
            foreach (var c in initialPosition.GetControllZone())
            {
                coords.Add(c);
                orient[c.Position] = c;
                parent[c] = initialPosition;
            }
            for (int i = 1; i < troop.MovePoints; i++)
                coords = GetNextLayer(troop.Player, coords);
            return new HashSet<VectorTwo>(coords.Select(c => c.Position));
        }

        private HashSet<OrientedCell> GetNextLayer(PlayerSide side, HashSet<OrientedCell> cells)
        {
            HashSet<OrientedCell> nextLayerCells = new HashSet<OrientedCell>();
            foreach (var cell in cells)
            {
                nextLayerCells.Add(cell);
                if (map.Get(cell.Position) != null) continue;
                foreach (var controllerCell in cell.GetControllZone())
                {
                    Troop encounter = map.Get(controllerCell.Position);
                    if (encounter == null || encounter.Player != side)
                    {
                        nextLayerCells.Add(controllerCell);
                        orient[controllerCell.Position] = controllerCell;
                        parent[controllerCell] = controllerCell;
                    }
                }
            }
            return nextLayerCells;
        }

        public List<int> GetDirections(VectorTwo start, VectorTwo end)
        {
            List<int> directions = new List<int>();
            OrientedCell coords = orient[end];
            while (coords.Position != start)
            {
                OrientedCell prevCoords = parent[coords];
                int direction = prevCoords.GetDirection(coords);
                directions.Add(direction);
                coords = prevCoords;
            }
            return directions;
        }
    }
}
