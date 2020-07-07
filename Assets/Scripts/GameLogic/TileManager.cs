﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;

    private Tilemap tilemap;
    private IEnumerable<Vector2Int> positions;

    private static Color transparent = new Color(255, 255, 255, 0);
    private static Color active = new Color(255, 255, 255, 127);


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

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        positions = new List<Vector2Int>();

        var poss = tilemap.cellBounds.allPositionsWithin;
        foreach (var pos in poss)
        {
            if (tilemap.GetTile(pos))
            {
                tilemap.SetTileFlags(pos, TileFlags.None);
                tilemap.SetColor(pos, transparent);
            }
        }
    }

    public static void ActivateTiles(IEnumerable<Vector2Int> positions)
    {
        DeactivateTiles();

        foreach(var pos in positions)
        {
            instance.tilemap.SetColor((Vector3Int)pos, active);
        }
        instance.positions = positions;
    }

    public static void DeactivateTiles()
    {
        foreach (var pos in instance.positions)
        {
            instance.tilemap.SetColor((Vector3Int)pos, transparent);
        }
        instance.positions = new List<Vector2Int>();
    }
}