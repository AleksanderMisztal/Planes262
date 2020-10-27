using System;
using System.Linq;
using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace Planes262.GameLogic
{
    public class PathFinder
    {
        private readonly TroopMap map;
        private readonly Board board;

        private PlayerSide side;

        private readonly HashSet<OrientedCell> reachableCells = new HashSet<OrientedCell>();
        private readonly Dictionary<OrientedCell, OrientedCell> parent = new Dictionary<OrientedCell, OrientedCell>();
        private readonly Dictionary<VectorTwo, OrientedCell> orient = new Dictionary<VectorTwo, OrientedCell>();
        private readonly Queue<Action> q = new Queue<Action>();

        public PathFinder(TroopMap map, Board board)
        {
            this.map = map;
            this.board = board;
        }


        public HashSet<VectorTwo> GetReachableCells(VectorTwo position)
        {
            ResetMembers();
            ITroop troop = map.Get(position);
            if (troop == null) throw new Exception("Troop was null!");
            return GetReachableCells(troop);
        }

        private void ResetMembers()
        {
            reachableCells.Clear();
            parent.Clear();
            orient.Clear();
        }

        private HashSet<VectorTwo> GetReachableCells(ITroop troop)
        {
            side = troop.Player;
            
            ITroop t;
            bool blocked = troop.ControlZone.All(c => !board.IsInside(c) || ((t = map.Get(c)) != null && t.Player == side));
            if (blocked)
            {
                OrientedCell oCell = new OrientedCell(troop.Position, troop.Orientation);
                for (int i = -1; i < 2; i++)
                {
                    VectorTwo cell = Hex.GetAdjacentHex(troop.Position, troop.Orientation + i);
                    OrientedCell neigh = new OrientedCell(cell, troop.Orientation + i);
                    orient[cell] = neigh;
                    parent[neigh] = oCell;
                }
                return new HashSet<VectorTwo>(troop.ControlZone);
            }
            
            OrientedCell initialPosition = new OrientedCell(troop.Position, troop.Orientation);
            q.Enqueue(() => AddReachableCells(initialPosition, troop.MovePoints));
            while (q.Count > 0) q.Dequeue()();
            return new HashSet<VectorTwo>(reachableCells.Select(c => c.Position));
        }

        private void AddReachableCells(OrientedCell sourceCell, int movePoints)
        {
            if (movePoints <= 0) return;
            foreach (OrientedCell oCell in sourceCell.GetControlZone())
            {
                if (reachableCells.Contains(oCell) || !board.IsInside(oCell.Position)) continue;
                AddCell(sourceCell, movePoints - 1, oCell);
            }
        }

        private void AddCell(OrientedCell sourceCell, int movePoints, OrientedCell oCell)
        {
            ITroop encounter = map.Get(oCell.Position);
            if (encounter == null || encounter.Player != side)
            {
                reachableCells.Add(oCell);
                parent[oCell] = sourceCell;
                if (!orient.ContainsKey(oCell.Position))
                {
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
