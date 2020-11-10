using System;
using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Dtos;
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

        public TroopDto[] SpawnWave(IEnumerable<TroopDto> wave)
        {
            foreach (TroopDto dto in wave)
            {
                dto.position = GetEmptyCell(dto.position.Get()).Dto();
                Troop troop = dto.Get();
                map.Add(troop.Position, troop);
                GetTroops(troop.Player).Add(troop);
            }
            return wave.ToArray();
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
