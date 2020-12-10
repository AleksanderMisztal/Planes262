using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Planes262.GameLogic;
using Planes262.HexSystem;
using UnityEngine;

namespace Planes262.Managers
{
    public class FogController
    {
        private readonly Board board;
        private readonly TroopMap troopMap;
        private readonly GameObject[,] fogTiles;

        private readonly HashSet<VectorTwo>[] evenNeighs;
        private readonly HashSet<VectorTwo>[] oddNeighs;
        private IEnumerable<GameObject> visibleTiles;

        public FogController(GameObject fogTile, Board board, GridBase gridBase, TroopMap troopMap)
        {
            this.board = board;
            this.troopMap = troopMap;
            fogTiles = new GameObject[board.xSize, board.ySize];
            visibleTiles = new List<GameObject>();
            
            for (int x = 0; x < board.xSize; x++)
            for (int y = 0; y < board.ySize; y++)
            {
                GameObject tile = Object.Instantiate(fogTile);
                tile.transform.position = gridBase.ToWorld(x, y);
                fogTiles[x, y] = tile;
            }
            
            evenNeighs = new HashSet<VectorTwo>[7];
            evenNeighs[0] = new HashSet<VectorTwo>{new VectorTwo(0, 0)};
            for (int i = 1; i < 7; i++)
            {
                evenNeighs[i] = new HashSet<VectorTwo>();
                int k = i;
                while (k --> 0)
                    foreach (VectorTwo pos in evenNeighs[i - 1].SelectMany(Hex.GetNeighbours))
                        evenNeighs[i].Add(pos);
            }
            
            oddNeighs = new HashSet<VectorTwo>[7];
            oddNeighs[0] = new HashSet<VectorTwo>{new VectorTwo(1, 0)};
            for (int i = 1; i < 7; i++)
            {
                oddNeighs[i] = new HashSet<VectorTwo>();
                int k = i;
                while (k --> 0)
                    foreach (VectorTwo pos in oddNeighs[i - 1].SelectMany(Hex.GetNeighbours))
                        oddNeighs[i].Add(pos);
            }
            VectorTwo dp = new VectorTwo(-1, 0);
            for (int i = 0; i < 7; i++)
                oddNeighs[i] = new HashSet<VectorTwo>(oddNeighs[i].Select(p => p + dp));

        }
        
        public void CreateFog(PlayerSide activePlayer)
        {
            HashSet<VectorTwo> newVisibleTiles = new HashSet<VectorTwo>();
            foreach (Troop t in troopMap.GetTroops(activePlayer))
            {
                VectorTwo position = t.Position;
                HashSet<VectorTwo>[] neighs = position.x % 2 == 0 ? evenNeighs : oddNeighs;
                if (t.IsFlak)
                    foreach (VectorTwo pos in neighs[2].Select(v => v + t.Position))
                        newVisibleTiles.Add(pos);
                else
                    foreach (VectorTwo pos in neighs[6].Select(v => v + t.Position))
                        newVisibleTiles.Add(pos);
            }

            foreach (GameObject tile in visibleTiles) 
                tile.SetActive(true);

            visibleTiles = newVisibleTiles.Where(board.IsInside).Select(t => fogTiles[t.x, t.y]);
            foreach (GameObject tile in visibleTiles)
                tile.SetActive(false);
        }
    }
}