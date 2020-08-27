using Planes262.GameLogic;
using Planes262.GameLogic.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Planes262.UnityLayer
{
    public class TileManager : MonoBehaviour
    {
        private static TileManager instance;

        [SerializeField] private Tile boardTile;
        [SerializeField] private Tile rangeTile;
        [SerializeField] private Tile clickTile;

        [SerializeField] private Tilemap boardTilemap;
        [SerializeField] private Tilemap rangeTilemap;
        [SerializeField] private Tilemap clickTilemap;

        private static bool isGridActive;

        private IEnumerable<VectorTwo> positions = new List<VectorTwo>();
        private IEnumerable<VectorTwo> path = new List<VectorTwo>();

        private static readonly Color transparent = new Color(255, 255, 255, 0);
        private static readonly Color active = new Color(0, 0, 255, 255);
        private static readonly Color activeBlocked = new Color(0, 0, 0, 127);
        private static readonly Color onPath = new Color(255, 0, 0, 127);


        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(this);
        }

        public static void CreateBoard(Board board)
        {
            for (int x = 0; x <= board.XMax; x++)
                for (int y = 0; y <= board.YMax; y++)
                    CreateTile(x, y);
            instance.boardTilemap.gameObject.SetActive(false);
        }

        private static void CreateTile(int x, int y)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);

            instance.boardTilemap.SetTile(pos, instance.boardTile);

            instance.rangeTilemap.SetTile(pos, instance.rangeTile);
            instance.rangeTilemap.SetTileFlags(pos, TileFlags.None);
            instance.rangeTilemap.SetColor(pos, transparent);

            instance.clickTilemap.SetTile(pos, instance.clickTile);
        }

        public void ShowHideGrid()
        {
            isGridActive = !isGridActive;
            boardTilemap.gameObject.SetActive(isGridActive);
        }

        public static void ActivateTiles(IEnumerable<VectorTwo> positions)
        {
            DeactivateTiles();
            foreach (VectorTwo pos in positions)
                instance.rangeTilemap.SetColor(pos.ToVector3Int(), active);
            instance.positions = positions;
        }

        public static void ActivateTilesBlocked(HashSet<VectorTwo> positions)
        {
            DeactivateTiles();
            foreach (VectorTwo pos in positions)
                instance.rangeTilemap.SetColor(pos.ToVector3Int(), activeBlocked);
            instance.positions = positions;
        }

        public static void HighlightPath(IEnumerable<VectorTwo> positions)
        {
            foreach (VectorTwo pos in instance.path)
                instance.rangeTilemap.SetColor(pos.ToVector3Int(), active);
            foreach (VectorTwo pos in positions)
                instance.rangeTilemap.SetColor(pos.ToVector3Int(), onPath);
            instance.path = positions;
        }

        public static void DeactivateTiles()
        {
            foreach (VectorTwo pos in instance.positions)
                instance.rangeTilemap.SetColor(pos.ToVector3Int(), transparent);
            foreach (VectorTwo pos in instance.path)
                instance.rangeTilemap.SetColor(pos.ToVector3Int(), transparent);
            instance.positions = new List<VectorTwo>();
            instance.path = new List<VectorTwo>();
        }
    }

    public static class VectorTwoConversion
    {
        public static Vector3Int ToVector3Int(this VectorTwo v) => new Vector3Int(v.X, v.Y, 0);
    }
}