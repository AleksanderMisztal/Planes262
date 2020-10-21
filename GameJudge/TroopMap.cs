using System;
using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge
{
    internal class TroopMap : TroopMapBase
    {
        private readonly Board board;

        public TroopMap(Board board)
        {
            this.board = board;
        }

        public IEnumerable<Troop> SpawnWave(IEnumerable<Troop> wave)
        {
            
            foreach (Troop troop in wave)
            {
                troop.AdjustPosition(GetEmptyCell(troop.Position));
                map.Add(troop.Position, troop);
                GetTroops(troop.Player).Add(troop);
            }
            return wave;
        }

        private VectorTwo GetEmptyCell(VectorTwo seedPosition)
        {
            if (Get(seedPosition) == null) return seedPosition;

            Queue<VectorTwo> q = new Queue<VectorTwo>();
            q.Enqueue(seedPosition);
            while (q.Count > 0)
            {
                VectorTwo position = q.Dequeue();
                if (Get(position) == null) return position;
                IEnumerable<VectorTwo> neighbours = Hex.GetNeighbours(seedPosition);
                foreach (VectorTwo neigh in neighbours)
                    if (board.IsInside(neigh))
                        q.Enqueue(neigh);
            }
            throw new Exception("Couldn't find an empty cell");
        }
    }
}
