using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Scripts.GameLogic;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;

    [SerializeField]
    private Tile boardTile;
    [SerializeField]
    private Tile rangeTile;
    [SerializeField]
    private Tile clickTile;

    [SerializeField]
    private Tilemap boardTilemap;
    [SerializeField]
    private Tilemap rangeTilemap;
    [SerializeField]
    private Tilemap clickTilemap;

    private static bool isGridActive = false;

    private IEnumerable<Vector2Int> positions = new List<Vector2Int>();
    private IEnumerable<Vector2Int> path = new List<Vector2Int>();

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

    public static void CreateBoard(BoardParams board)
    {
        Debug.Log("Creating the board");
        for (int x = board.xMin; x <= board.xMax; x++)
        {
            for (int y = board.yMin; y <= board.yMax; y++)
            {
                var pos = new Vector3Int(x, y, 0);

                instance.boardTilemap.SetTile(pos, instance.boardTile);

                instance.rangeTilemap.SetTile(pos, instance.rangeTile);
                instance.rangeTilemap.SetTileFlags(pos, TileFlags.None);
                instance.rangeTilemap.SetColor(pos, transparent);


                instance.clickTilemap.SetTile(pos, instance.clickTile);
            }
        }
        instance.boardTilemap.gameObject.SetActive(false);
    }

    public void ShowHideGrid()
    {
        isGridActive = !isGridActive;
        boardTilemap.gameObject.SetActive(isGridActive);
    }

    public static void ActivateTiles(IEnumerable<Vector2Int> positions)
    {
        DeactivateTiles();

        foreach(var pos in positions)
        {
            instance.rangeTilemap.SetColor((Vector3Int)pos, active);
        }
        instance.positions = positions;
    }

    internal static void ActivateTilesBlocked(HashSet<Vector2Int> positions)
    {
        DeactivateTiles();

        foreach (var pos in positions)
        {
            instance.rangeTilemap.SetColor((Vector3Int)pos, activeBlocked);
        }
        instance.positions = positions;
    }

    public static void HighlightPath(IEnumerable<Vector2Int> positions)
    {
        foreach (var pos in instance.path)
        {
            instance.rangeTilemap.SetColor((Vector3Int)pos, active);
        }

        foreach (var pos in positions)
        {
            instance.rangeTilemap.SetColor((Vector3Int)pos, onPath);
        }
        instance.path = positions;
    }

    public static void DeactivateTiles()
    {
        foreach (var pos in instance.positions)
        {
            instance.rangeTilemap.SetColor((Vector3Int)pos, transparent);
        }
        foreach (var pos in instance.path)
        {
            instance.rangeTilemap.SetColor((Vector3Int)pos, transparent);
        }
        instance.positions = new List<Vector2Int>();
        instance.path = new List<Vector2Int>();
    }
}
