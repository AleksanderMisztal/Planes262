using System;
using System.Linq;
using System.Collections.Generic;
using Planes262.GameLogic.Utils;
using Planes262.GameLogic.Exceptions;

namespace Planes262.GameLogic
{
    public class PathFinder
    {
        private readonly TroopMap map;

        private PlayerSide side;

        private HashSet<OrientedCell> reachableCells = new HashSet<OrientedCell>();
        private Dictionary<OrientedCell, OrientedCell> parent = new Dictionary<OrientedCell, OrientedCell>();
        private Dictionary<VectorTwo, OrientedCell> orient = new Dictionary<VectorTwo, OrientedCell>();
        private readonly Queue<Action> q = new Queue<Action>();

        public PathFinder(TroopMap map)
        {
            this.map = map;
        }


        public HashSet<VectorTwo> GetReachableCells(VectorTwo position)
        {
            ResetMembers();
            Troop troop = map.Get(position);
            if (troop == null) throw new PathFindingException("No troop on this hex!");
            return GetReachableCells(troop);
        }

        private void ResetMembers()
        {
            reachableCells = new HashSet<OrientedCell>();
            parent = new Dictionary<OrientedCell, OrientedCell>();
            orient = new Dictionary<VectorTwo, OrientedCell>();
        }

        private HashSet<VectorTwo> GetReachableCells(Troop troop)
        {
            side = troop.Player;
            OrientedCell initialPosition = new OrientedCell(troop.Position, troop.Orientation);
            q.Enqueue(() => AddReachableCells(initialPosition, troop.MovePoints));
            while (q.Count > 0) q.Dequeue()();
            return new HashSet<VectorTwo>(reachableCells.Select(c => c.Position));
        }

        private void AddReachableCells(OrientedCell sourceCell, int movePoints)
        {
            if (movePoints <= 0) return;
            foreach (OrientedCell oCell in sourceCell.GetControllZone())
            {
                if (reachableCells.Contains(oCell)) continue;
                AddCell(sourceCell, movePoints - 1, oCell);
            }
        }

        private void AddCell(OrientedCell sourceCell, int movePoints, OrientedCell oCell)
        {
            Troop encounter = map.Get(oCell.Position);
            if (encounter == null || encounter.Player != side)
            {
                reachableCells.Add(oCell);
                if (!orient.TryGetValue(oCell.Position, out _))
                {
                    parent[oCell] = sourceCell;
                    orient[oCell.Position] = oCell;
                }
            }
            if (encounter == null)
                q.Enqueue(() => AddReachableCells(oCell, movePoints));
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
            directions.Reverse();
            return directions;
        }
    }
}
