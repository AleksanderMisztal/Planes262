using System;
using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Hex = GameJudge.Utils.Hex;

namespace GameJudge
{
    internal class TroopMap : TroopMapBase
    {
        private readonly Board board;

        public TroopMap(Board board)
        {
            this.board = board;
        }

        public IEnumerable<TroopDto> SpawnWave(IEnumerable<TroopDto> wave)
        {
            foreach (TroopDto t in wave)
            {
                t.AdjustPosition(GetEmptyCell(t.Position));
                
                Troop troop = new Troop(t);
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
                VectorTwo[] neighbours = Hex.GetNeighbours(seedPosition);
                foreach (VectorTwo neigh in neighbours)
                    if (board.IsInside(neigh))
                        q.Enqueue(neigh);
            }
            throw new Exception("Couldn't find an empty cell");
        }
    }
}
