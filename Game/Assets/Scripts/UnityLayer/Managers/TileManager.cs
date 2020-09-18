using System.Collections.Generic;
using GameDataStructures;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Planes262.UnityLayer.Managers
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private Tile boardTile;
        [SerializeField] private Tile rangeTile;
        [SerializeField] private Tile clickTile;

        [SerializeField] private Tilemap boardTilemap;
        [SerializeField] private Tilemap rangeTilemap;
        [SerializeField] private Tilemap clickTilemap;

        private bool isGridActive;

        private IEnumerable<VectorTwo> activePositions = new List<VectorTwo>();
        private IEnumerable<VectorTwo> highlightedPath = new List<VectorTwo>();

        private static readonly Color transparent = new Color(255, 255, 255, 0);
        private static readonly Color active = new Color(0, 0, 255, 255);
        private static readonly Color activeBlocked = new Color(0, 0, 0, 127);
        private static readonly Color onPath = new Color(255, 0, 0, 127);


        public void CreateBoard(Board board)
        {
            for (int x = 0; x <= board.XMax; x++)
                for (int y = 0; y <= board.YMax; y++)
                    CreateTile(x, y);
            boardTilemap.gameObject.SetActive(false);
        }

        private void CreateTile(int x, int y)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);

            boardTilemap.SetTile(pos, boardTile);

            rangeTilemap.SetTile(pos, rangeTile);
            rangeTilemap.SetTileFlags(pos, TileFlags.None);
            rangeTilemap.SetColor(pos, transparent);

            clickTilemap.SetTile(pos, clickTile);
        }

        public void ShowHideGrid()
        {
            isGridActive = !isGridActive;
            boardTilemap.gameObject.SetActive(isGridActive);
        }

        public void ActivateTiles(IEnumerable<VectorTwo> toActivate)
        {
            DeactivateTiles();
            foreach (VectorTwo pos in toActivate)
                rangeTilemap.SetColor(pos.ToVector3Int(), active);
            activePositions = toActivate;
        }

        public void ActivateTilesBlocked(HashSet<VectorTwo> toActivate)
        {
            DeactivateTiles();
            foreach (VectorTwo pos in toActivate)
                rangeTilemap.SetColor(pos.ToVector3Int(), activeBlocked);
            activePositions = toActivate;
        }

        public void HighlightPath(IEnumerable<VectorTwo> toHighlight)
        {
            foreach (VectorTwo pos in highlightedPath)
                rangeTilemap.SetColor(pos.ToVector3Int(), active);
            foreach (VectorTwo pos in toHighlight)
                rangeTilemap.SetColor(pos.ToVector3Int(), onPath);
            highlightedPath = toHighlight;
        }

        public void DeactivateTiles()
        {
            foreach (VectorTwo pos in activePositions)
                rangeTilemap.SetColor(pos.ToVector3Int(), transparent);
            foreach (VectorTwo pos in highlightedPath)
                rangeTilemap.SetColor(pos.ToVector3Int(), transparent);
            this.activePositions = new List<VectorTwo>();
            highlightedPath = new List<VectorTwo>();
        }
    }

    public static class VectorTwoConversion
    {
        public static Vector3Int ToVector3Int(this VectorTwo v) => new Vector3Int(v.X, v.Y, 0);
    }
}