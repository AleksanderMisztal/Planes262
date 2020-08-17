using GameServer.GameLogic;
using GameServer.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;

    [SerializeField] private Tile boardTile;
    [SerializeField] private Tile rangeTile;
    [SerializeField] private Tile clickTile;

    [SerializeField] private Tilemap boardTilemap;
    [SerializeField] private Tilemap rangeTilemap;
    [SerializeField] private Tilemap clickTilemap;

    private static bool isGridActive = false;

    private IEnumerable<VectorTwo> positions = new List<VectorTwo>();
    private IEnumerable<VectorTwo> path = new List<VectorTwo>();

    private static Color transparent = new Color(255, 255, 255, 0);
    private static Color active = new Color(0, 0, 255, 255);
    private static Color activeBlocked = new Color(0, 0, 0, 127);
    private static Color onPath = new Color(255, 0, 0, 127);


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying this...");
            Destroy(this);
        }
    }

    public static void CreateBoard(Board board)
    {
        Debug.Log("Creating the board");
        for (int x = 0; x <= board.xMax; x++)
            for (int y = 0; y <= board.yMax; y++)
                CreateTile(x, y);
        instance.boardTilemap.gameObject.SetActive(false);
    }

    private static void CreateTile(int x, int y)
    {
        var pos = new Vector3Int(x, y, 0);

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
        foreach(var pos in positions)
            instance.rangeTilemap.SetColor((Vector3Int)pos, active);
        instance.positions = positions;
    }

    public static void ActivateTilesBlocked(HashSet<VectorTwo> positions)
    {
        DeactivateTiles();
        foreach (var pos in positions)
            instance.rangeTilemap.SetColor((Vector3Int)pos, activeBlocked);
        instance.positions = positions;
    }

    public static void HighlightPath(IEnumerable<VectorTwo> positions)
    {
        foreach (var pos in instance.path)
            instance.rangeTilemap.SetColor((Vector3Int)pos, active);
        foreach (var pos in positions)
            instance.rangeTilemap.SetColor((Vector3Int)pos, onPath);
        instance.path = positions;
    }

    public static void DeactivateTiles()
    {
        foreach (var pos in instance.positions)
            instance.rangeTilemap.SetColor((Vector3Int)pos, transparent);
        foreach (var pos in instance.path)
            instance.rangeTilemap.SetColor((Vector3Int)pos, transparent);
        instance.positions = new List<VectorTwo>();
        instance.path = new List<VectorTwo>();
    }
}
